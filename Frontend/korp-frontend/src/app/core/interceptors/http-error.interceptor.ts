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
  constructor(private notify: NotificationService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        const alreadyHandled =
          (typeof err.error === 'object' && err.error?.handled === true);

        if (!alreadyHandled) {
          let msg: string;

          if (typeof err.error === 'string' && err.error.trim() !== '') {
            msg = err.error;
          } else if (err.error?.message) {
            msg = err.error.message;
          } else {
            msg = '⚠️ Sistema indisponível. Tente novamente mais tarde.';
          }

          this.notify.showError(msg);
        }

        catchError((err: HttpErrorResponse) => {
          const alreadyHandled =
            typeof err.error === 'object' && err.error?.handled === true;

          if (!alreadyHandled) {
            let msg: string;

            if (typeof err.error === 'string' && err.error.trim() !== '') {
              msg = err.error;
            } else if (err.error?.message) {
              msg = err.error.message;
            } else {
              msg = '⚠️ Sistema indisponível. Tente novamente mais tarde.';
            }

            this.notify.showError(msg);
          }
          return throwError(() => err);
        });


        return throwError(() => err);
      })
    );
  }
}
