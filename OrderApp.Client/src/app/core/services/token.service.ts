import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { environment } from 'src/environments/environment.development';
import { tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TokenService {
  constructor(private httpClient: HttpClient, private storageService: LocalStorageService) {}

  refreshToken() {
    const refreshToken = this.storageService.getItem('refreshToken');
    if (refreshToken) {
      return this.httpClient
        .post<any>(`${environment.apiUrl}/Auth/refresh-token`, { refreshToken })
        .pipe(
          tap((res) => {
            if (res.success) {
              this.storageService.setToken(res.data.token);
              this.storageService.setItem('refreshToken', res.data.refreshToken);
            }
          })
        );
    } else {
      console.error('No refresh token found.');
      return;
    }
  }
}
