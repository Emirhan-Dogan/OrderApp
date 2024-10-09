import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { APIConstant } from 'src/app/shared/constants/APIConstant';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ColorService {
  constructor(private http: HttpClient) {}

  getAll<T=any>(): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.COLORS.GET_ALL)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById<T=any>(id: number): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.COLORS.GET_BY_ID+`?Id=${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  add<Req=any,Res=any>(object: Req): Observable<Res> {
    return this.http.post<Res>(environment.apiUrl+APIConstant.COLORS.ADD, object)
      .pipe(
        catchError(this.handleError)
      );
  }

  update<Req=any,Res=any>(id: number, object: Req): Observable<Res> {
    return this.http.put<Res>(environment.apiUrl+APIConstant.COLORS.UPDATE, object)
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(id: number): Observable<void> {
    const options = {
      body: { id: id },
    };
    
    return this.http.delete<void>(environment.apiUrl+APIConstant.COLORS.DELETE, options)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    console.error('Service Error', error);
    return throwError(() => new Error(error));
  }
}
