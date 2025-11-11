import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { ProductsListComponent } from './components/products/list/products-list.component';
import { ProductFormComponent } from './components/products/form/product-form.component';
import { InvoicesListComponent } from './components/invoices/list/invoices-list.component';
import { InvoiceFormComponent } from './components/invoices/form/invoice-form.component';
import { InvoiceItemsComponent } from './components/invoices/itens/invoice-items.component';
import { LoaderComponent } from './components/shared/loader/loader.component';
import { ToastComponent } from './components/shared/Toast/toast.component';


import { HttpErrorInterceptor } from './core/interceptors/http-error.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    ProductsListComponent,
    ProductFormComponent,
    InvoicesListComponent,
    InvoiceFormComponent,
    InvoiceItemsComponent,
    ToastComponent,
    LoaderComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
