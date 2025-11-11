import { Component, OnInit } from '@angular/core';
import { InvoiceService } from '../../../services/invoice.service';
import { NotificationService } from '../../../core/services/notification.service';
import { Invoice } from '../../../models/invoice.model';

@Component({
  selector: 'app-invoices-list',
  templateUrl: './invoices-list.component.html',
  styleUrls: ['./invoices-list.component.scss']
})
export class InvoicesListComponent implements OnInit {
  invoices: Invoice[] = [];
  printingId: number | null = null;
  deletingId: number | null = null;

  constructor(
    private invoiceService: InvoiceService,
    private notify: NotificationService
  ) { }

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.invoiceService.getAll().subscribe({
      next: data => this.invoices = data,
      error: err => {
        console.error(err);
        this.notify?.showError?.('Erro ao carregar notas fiscais.');
      }
    });
  }

  print(id: number): void {
  if (!id) return;
  if (!confirm('Deseja imprimir esta nota?')) return;

  this.printingId = id;

  this.invoiceService.getWithItems(id.toString()).subscribe({
    next: invWithItems => {
      this.invoiceService.print(id.toString()).subscribe({
        next: () => {
          const inv = this.invoices.find(i => i.id === id);
          if (inv) inv.status = 'Fechada';
          this.notify?.showSuccess('✅ Nota impressa com sucesso!');
          this.printingId = null;
        },
        error: err => {
          this.printingId = null;

          if (err.status === 400 && err.error) {
            const regex = /produto\s+([^\s]+)/i;
            const match = typeof err.error === 'string' ? err.error.match(regex) : null;
            const productCode = match ? match[1] : '';

            const msg = (typeof err.error === 'string' ? err.error : '').toLowerCase();

            if (msg.includes('estoque') || msg.includes('saldo')) {
              let productName = productCode;
              const item = invWithItems.items?.find(it => it.productCode === productCode);
              if (item) productName = item.productDescription;

              err.error = { original: err.error, handled: true };
              this.notify.showError(`❌ Produto "${productName}" sem saldo suficiente no estoque.`);
              return;
            }
          }

          console.error('Erro ao imprimir nota:', err);
          err.error = { original: err.error, handled: true };
          this.notify.showError('⚠️ Falha ao imprimir a nota. Tente novamente mais tarde.');
        }
      });
    }
  });
}

  delete(id: number): void {
    if (!id) return;
    const inv = this.invoices.find(i => i.id === id);
    if (!inv) return;
    if (inv.status !== 'Aberta') {
      this.notify?.showError?.('Apenas notas abertas podem ser excluídas.');
      return;
    }
    if (!confirm('Tem certeza que deseja excluir esta nota aberta?')) return;

    this.deletingId = id;
    this.invoiceService.delete(id.toString()).subscribe({
      next: () => {
        this.notify?.showSuccess?.('Nota excluída com sucesso!');
        this.deletingId = null;
        this.loadInvoices();
      },
      error: err => {
        console.error('Erro ao excluir nota:', err);
        if (typeof err.error === 'string') {
          err.error = { original: err.error, handled: true };
        } else if (err.error) {
          err.error.handled = true;
        }
        this.notify?.showError?.('Falha ao excluir a nota.');
        this.deletingId = null;
      }
    });
  }
}
