import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvoiceService } from '../../../services/invoice.service';
import { NotificationService } from '../../../core/services/notification.service';
import { ProductService } from '../../../services/product.service';
import { Invoice, InvoiceItem } from '../../../models/invoice.model';

@Component({
  selector: 'app-invoice-items',
  templateUrl: './invoice-items.component.html',
  styleUrls: ['./invoice-items.component.scss']
})
export class InvoiceItemsComponent implements OnInit {
  invoice!: Invoice;
  products: any[] = [];
  loading = true;
  deletingId: string | null = null;
  editingItemId: number | null = null;
  editedQuantity: number | null = null;
  saving = false;
  showAdd = false;

  constructor(
    private route: ActivatedRoute,
    private invoiceService: InvoiceService,
    private notify: NotificationService,
    private router: Router,
    private productService: ProductService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.loadInvoice(id);

    this.productService.loadAll().subscribe();
    this.productService.getAllStream().subscribe({
      next: data => this.products = data,
      error: err => {
        console.error('Erro ao carregar produtos:', err);
        this.notify.showError('Erro ao carregar lista de produtos.');
      }
    });
  }

  loadInvoice(id: string): void {
    this.invoiceService.getWithItems(id).subscribe({
      next: data => {
        this.invoice = data;
        this.loading = false;
      },
      error: err => {
        console.error('Erro ao carregar nota:', err);
        this.notify.showError('Erro ao carregar nota fiscal.');
        this.loading = false;
      }
    });
  }

  addItem(product: any): void {
    if (!this.invoice) return;

    this.productService.getByCode(product.code).subscribe({
      next: (prod) => {
        const estoque = prod?.stockQuantity ?? 0;

        if (estoque <= 0) {
          this.notify.showError(`❌ Produto "${product.description}" está sem estoque.`);
          return;
        }

        const newItem: InvoiceItem = {
          productCode: product.code,
          productDescription: product.description,
          quantity: 1,
          invoiceId: this.invoice.id
        };

        this.invoiceService.addItem(newItem).subscribe({
          next: item => {
            this.invoice.items.push(item);
            this.notify.showSuccess(`✅ Produto "${product.description}" adicionado à nota.`);
            this.showAdd = false;
          },
          error: err => {
            console.error('Erro ao adicionar item:', err);
            this.notify.showError('Falha ao adicionar produto à nota.');
          }
        });
      },
      error: (err) => {
        console.error('Erro ao verificar estoque:', err);
        this.notify.showError('Falha ao verificar estoque do produto.');
      }
    });
  }


  deleteItem(id: number): void {
    if (!id) return;
    if (!confirm('Deseja excluir este item?')) return;

    this.deletingId = id.toString();

    this.invoiceService.deleteItem(id).subscribe({
      next: () => {
        this.notify.showSuccess('Item excluído com sucesso!');
        this.invoice.items = this.invoice.items.filter(i => i.id !== id);
        this.deletingId = null;
      },
      error: err => {
        console.error('Erro ao excluir item:', err);
        this.notify.showError('Falha ao excluir o item.');
        this.deletingId = null;
      }
    });
  }

  editItem(item: InvoiceItem): void {
    this.editingItemId = item.id!;
    this.editedQuantity = item.quantity;
  }

  cancelEdit(): void {
    this.editingItemId = null;
    this.editedQuantity = null;
  }

  saveEdit(item: InvoiceItem): void {
    if (!this.editedQuantity || this.editedQuantity <= 0) {
      this.notify.showError('Informe uma quantidade válida.');
      return;
    }

    if (!item.productCode) {
      this.notify.showError('Produto inválido.');
      return;
    }

    this.saving = true;

    this.productService.getByCode(item.productCode).subscribe({
      next: (product) => {
        const estoque = product?.stockQuantity ?? 0;

        if (this.editedQuantity! > estoque) {
          this.notify.showError(`Quantidade não pode exceder o estoque (${estoque} disponíveis).`);
          this.saving = false;
          return;
        }

        const updatedItem = { ...item, quantity: this.editedQuantity };

        this.invoiceService.updateItem(updatedItem).subscribe({
          next: () => {
            this.notify.showSuccess('Item atualizado com sucesso!');
            item.quantity = this.editedQuantity!;
            this.cancelEdit();
            this.saving = false;
          },
          error: (err) => {
            console.error('Erro ao atualizar item:', err);
            this.notify.showError('Falha ao atualizar item.');
            this.saving = false;
          },
        });
      },
      error: (err) => {
        console.error('Erro ao buscar produto:', err);
        this.notify.showError('Falha ao verificar o estoque do produto.');
        this.saving = false;
      },
    });
  }
}
