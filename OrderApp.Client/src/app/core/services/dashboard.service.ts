import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { APIConstant } from 'src/app/shared/constants/APIConstant';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private http: HttpClient) {}

  getAllData<T=any>(): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.DASHBOARD.GET_DATA)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    console.error('Service Error', error);
    return throwError(() => new Error(error));
  }
}
