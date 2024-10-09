import { Component, EventEmitter, Output } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [MatToolbarModule, MatIconModule, MatButtonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  @Output() menuToggle = new EventEmitter<void>();

  constructor(private router:Router, private authService:AuthService){}

  toggleSidenav() {
    console.log('Menu button clicked'); // Bu satırı ekleyin
    this.menuToggle.emit();
  }
  logout(){
    this.authService.logOut();
    this.router.navigate(['/login'])
  }
}
