import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Product } from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {
  private api = 'https://localhost:7020/api/Product';
  private products$ = new BehaviorSubject<Product[]>([]);

  constructor(private http: HttpClient) {}

  loadAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.api).pipe(
      tap(p => this.products$.next(p))
    );
  }

  getAllStream(): Observable<Product[]> {
    return this.products$.asObservable();
  }

  getById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.api}/${id}`);
  }

  create(product: Product): Observable<Product> {
    return this.http.post<Product>(this.api, product);
  }

  update(product: Product): Observable<void> {
    return this.http.put<void>(`${this.api}/${product.id}`, product);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }

  getByCode(code: string): Observable<Product> {
  return this.http.get<Product>(`${this.api}/code/${code}`);
}

}
