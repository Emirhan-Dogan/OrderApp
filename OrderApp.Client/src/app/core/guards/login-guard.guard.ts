import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service'; // AuthService'in doğru yolu

@Injectable({ providedIn: 'root' })
export class LoginGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    
    if (this.authService.loggedIn()) {
      return true;
    } else {
      console.log('logine döndü');
      // Giriş yapmamışsa kullanıcıyı giriş sayfasına yönlendir
      this.router.navigate(['/login']);
      return false;
    }
  }
}
