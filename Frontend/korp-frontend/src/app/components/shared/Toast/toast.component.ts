import { Component, OnInit } from '@angular/core';
import { NotificationService, ToastMessage } from '../../../core/services/notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-toast',
  template: `
    <div class="toast-container">
      <div *ngFor="let toast of toasts" [ngClass]="toast.type" class="toast">
        {{ toast.text }}
      </div>
    </div>
  `,
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {
  toasts: ToastMessage[] = [];
  private sub!: Subscription;

  constructor(private notify: NotificationService) {}

  ngOnInit(): void {
    this.sub = this.notify.toastState.subscribe(toast => {
      this.toasts.push(toast);
      setTimeout(() => this.toasts.shift(), 4000);
    });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
