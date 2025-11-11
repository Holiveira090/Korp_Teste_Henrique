export interface InvoiceItem {
  id?: number;
  productCode: string;
  productDescription: string;
  quantity: number;
  invoiceId?: number;
}


export interface Invoice {
  id?: number;
  sequentialNumber: number;
  status: string;
  createdAt: string;
  items: InvoiceItem[];
}
