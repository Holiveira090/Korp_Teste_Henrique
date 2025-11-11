import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.scss']
})
export class ProductsListComponent implements OnInit {
  products: Product[] = [];
  backendUnavailable = false;

  constructor(
    private productService: ProductService,
    private router: Router,
    private notify: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.loadAll().subscribe({
      next: () => {
        this.backendUnavailable = false;
      },
      error: err => {
        console.error('Erro ao carregar produtos:', err);
        this.backendUnavailable = true;
      }
    });

    this.productService.getAllStream().subscribe(p => {
      this.products = p;
    });
  }

  editProduct(product: Product): void {
    this.router.navigate(['/products/edit', product.id]);
  }

  deleteProduct(id?: string): void {
    if (!id) return;
    if (!confirm('Tem certeza que deseja deletar este produto?')) return;

    this.productService.delete(id).subscribe({
      next: () => {
        this.notify.showSuccess('✅ Produto deletado com sucesso!');
        this.loadProducts();
      },
      error: err => {
        console.error('Erro ao deletar produto:', err);
        this.notify.showError('⚠️ Falha ao deletar o produto. Tente novamente mais tarde.');
      }
    });
  }
}
