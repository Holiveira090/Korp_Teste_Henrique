import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(private notify: NotificationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        const alreadyHandled =
          (typeof err.error === 'object' && err.error?.handled === true);

        if (!alreadyHandled) {
          const msg =
            (typeof err.error === 'string' ? err.error : err.error?.message) ||
            err.message ||
            'Erro de comunicação com o servidor';

          this.notify.showError(msg);
        }

        return throwError(() => err);
      })
    );
  }
}
