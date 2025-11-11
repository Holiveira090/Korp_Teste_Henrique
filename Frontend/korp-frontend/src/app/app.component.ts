import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <div class="app-shell">
      <header>
        <div class="logo">
          <h1>Korp</h1>
          <span>Sistema de Notas Fiscais</span>
        </div>
        <nav class="navbar">
  <a routerLink="/products" 
     routerLinkActive="active" 
     [routerLinkActiveOptions]="{ exact: true }">
     📦 Produtos
  </a>

  <a routerLink="/products/new" 
     routerLinkActive="active">
     ➕ Novo Produto
  </a>

  <a routerLink="/invoices" 
     routerLinkActive="active" 
     [routerLinkActiveOptions]="{ exact: true }">
     🧾 Notas
  </a>

  <a routerLink="/invoices/new" 
     routerLinkActive="active">
     ➕ Nova Nota
  </a>
</nav>

      </header>

      <main>
        <section>
          <router-outlet></router-outlet>
        </section>
      </main>
    </div>

    <!-- Toast global -->
    <app-toast></app-toast>
  `,
  styleUrls: ['./app.component.scss']
})
export class AppComponent { }
