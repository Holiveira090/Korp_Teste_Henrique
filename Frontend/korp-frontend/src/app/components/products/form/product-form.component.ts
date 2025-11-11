import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss']
})
export class ProductFormComponent {
  model: any = {};
  saving = false;
  id: number | null = null;

  constructor(
    private http: HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private notify: NotificationService
  ) {}

  ngOnInit() {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (this.id) {
      this.http.get(`https://localhost:7020/api/Product/${this.id}`).subscribe({
        next: (data: any) => (this.model = data),
        error: (err) => {
          console.error('Erro ao carregar produto:', err);
          this.notify.showError('⚠️ Erro ao carregar produto.');
        }
      });
    }
  }

  save() {
    this.saving = true;
    const request = this.id
      ? this.http.put(`https://localhost:7020/api/Product/${this.id}`, this.model)
      : this.http.post('https://localhost:7020/api/Product', this.model);

    request.subscribe({
      next: () => {
        this.saving = false;
        this.notify.showSuccess('✅ Produto salvo com sucesso!');
        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.saving = false;
        console.error('Erro ao salvar produto:', err);

        if (err.status === 400 || err.status === 409) {
          this.notify.showError('❌ Produto já existe!');
        } else {
          this.notify.showError('⚠️ Falha ao salvar produto. Tente novamente.');
        }
      }
    });
  }
}
