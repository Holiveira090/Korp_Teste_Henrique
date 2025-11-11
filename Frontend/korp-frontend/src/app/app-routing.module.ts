import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsListComponent } from './components/products/list/products-list.component';
import { ProductFormComponent } from './components/products/form/product-form.component';
import { InvoicesListComponent } from './components/invoices/list/invoices-list.component';
import { InvoiceFormComponent } from './components/invoices/form/invoice-form.component';
import { InvoiceItemsComponent } from './components/invoices/itens/invoice-items.component';

const routes: Routes = [
  { path: 'products', component: ProductsListComponent },
  { path: 'products/new', component: ProductFormComponent },
  { path: 'invoices', component: InvoicesListComponent },
  { path: 'invoices/new', component: InvoiceFormComponent },
  { path: 'invoices/:id/items', component: InvoiceItemsComponent },
  { path: '', redirectTo: '/products', pathMatch: 'full' },
  { path: 'products/edit/:id', component: ProductFormComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
