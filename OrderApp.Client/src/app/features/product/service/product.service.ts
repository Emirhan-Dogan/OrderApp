import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { APIConstant } from 'src/app/shared/constants/APIConstant';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  constructor(private http: HttpClient) {}

  getAll<T=any>(): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.PRODUCTS.GET_ALL)
      .pipe(
        catchError(this.handleError)
      );
  }

  getAllPagination<T=any>(page:number): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.PRODUCTS.GET_ALL_PAGINATION+`?page=${page}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getAllForStorage<T=any>(StorageId:number): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.PRODUCTS.GET_ALL_FOR_STORAGE+`?StorageId=${StorageId}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  getById<T=any>(id: number): Observable<T> {
    return this.http.get<T>(environment.apiUrl+APIConstant.PRODUCTS.GET_BY_ID+`?Id=${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  add<Req=any,Res=any>(object: Req): Observable<Res> {
    return this.http.post<Res>(environment.apiUrl+APIConstant.PRODUCTS.ADD, object, {responseType:'text' as 'json'})
      .pipe(
        catchError(this.handleError)
      );
  }

  update<Req=any,Res=any>(object: Req): Observable<Res> {
    return this.http.put<Res>(environment.apiUrl+APIConstant.PRODUCTS.UPDATE, object)
      .pipe(
        catchError(this.handleError)
      );
  }

  delete(id: number): Observable<void> {
    const options = {
      body: { id: id },
      responseType:'text' as 'json'
    };
    
    return this.http.delete<void>(environment.apiUrl+APIConstant.PRODUCTS.DELETE, options)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    console.error('Service Error', error);
    return throwError(() => new Error(error));
  }

}
