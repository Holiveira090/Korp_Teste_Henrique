import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface ToastMessage {
  type: 'success' | 'error' | 'info';
  text: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private toastSubject = new Subject<ToastMessage>();
  toastState = this.toastSubject.asObservable();

  showSuccess(message: string) {
    this.toastSubject.next({ type: 'success', text: message });
  }

  showError(message: string) {
    this.toastSubject.next({ type: 'error', text: message });
  }

  showInfo(message: string) {
    this.toastSubject.next({ type: 'info', text: message });
  }
}
