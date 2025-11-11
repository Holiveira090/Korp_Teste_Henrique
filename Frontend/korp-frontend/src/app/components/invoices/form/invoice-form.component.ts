import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { InvoiceService } from '../../../services/invoice.service';
import { ProductService } from '../../../services/product.service';
import { Invoice } from '../../../models/invoice.model';
import { Product } from '../../../models/product.model';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-invoice-form',
  templateUrl: './invoice-form.component.html',
  styleUrls: ['./invoice-form.component.scss']
})
export class InvoiceFormComponent implements OnInit {
  products: Product[] = [];
  model: Invoice = {
    sequentialNumber: 0,
    status: 'Aberta',
    createdAt: new Date().toISOString(),
    items: []
  };

  constructor(
    private invoiceService: InvoiceService,
    private notify: NotificationService,
    private productService: ProductService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.productService.loadAll().subscribe();
    this.productService.getAllStream().subscribe(p => (this.products = p));
  }

  addItem(productCode: string) {
  const product = this.products.find(p => p.code === productCode);
  if (!product) return;

  const existing = this.model.items.find(i => i.productCode === product.code);
  const totalQuantity = (existing?.quantity || 0) + 1;

  if (product.stockQuantity < totalQuantity) {
    this.notify.showError(`❌ Estoque insuficiente para o produto "${product.description}"`);
    return;
  }

  if (existing) {
    existing.quantity++;
  } else {
    this.model.items.push({
      productCode: product.code,
      productDescription: product.description,
      quantity: 1
    });
  }
}


  save() {
    console.log('📦 Enviando invoice:', this.model);
    this.invoiceService.create(this.model).subscribe(() => {
      this.router.navigate(['/invoices']);
    });
  }

  decreaseItem(productCode: string) {
  const itemIndex = this.model.items.findIndex((it: any) => it.productCode === productCode);
  if (itemIndex === -1) return;

  const item = this.model.items[itemIndex];
  item.quantity -= 1;

  if (item.quantity <= 0) {
    this.model.items.splice(itemIndex, 1);
  }
}

}
