import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice, InvoiceItem } from '../models/invoice.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private apiInvoice = 'https://localhost:7011/api/Invoice';
  private apiInvoiceItem = 'https://localhost:7011/api/InvoiceItem'; 

  constructor(private http: HttpClient) { }

  create(invoice: Invoice): Observable<Invoice> {
    return this.http.post<Invoice>(`${this.apiInvoice}`, invoice);
  }

  print(invoiceId: string): Observable<any> {
    return this.http.put(`${this.apiInvoice}/${invoiceId}/print`, null);
  }

  getAll(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(`${this.apiInvoice}`);
  }

  getWithItems(id: string): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.apiInvoice}/WithItems/${id}`);
  }

  delete(invoiceId: string): Observable<any> {
    return this.http.delete(`${this.apiInvoice}/${invoiceId}`);
  }

  deleteItem(id: number): Observable<any> {
    return this.http.delete(`${this.apiInvoiceItem}/${id}`);
  }

  updateItem(item: InvoiceItem): Observable<any> {
    return this.http.put(`${this.apiInvoiceItem}/${item.id}`, item);
  }

  addItem(item: InvoiceItem): Observable<InvoiceItem> {
    return this.http.post<InvoiceItem>(`${this.apiInvoiceItem}`, item);
  }
}