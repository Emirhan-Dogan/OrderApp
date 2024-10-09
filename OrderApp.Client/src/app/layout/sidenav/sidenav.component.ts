import { Component } from '@angular/core';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    MatInputModule,
  ],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.scss',
})
export class SidenavComponent {
  userName: string = 'User Name';
  userEmail: string = 'Roles';
  userImage:String='https://static.vecteezy.com/system/resources/thumbnails/005/129/844/small_2x/profile-user-icon-isolated-on-white-background-eps10-free-vector.jpg'
  
  constructor(private authService:AuthService) {
    this.userName = <string>localStorage.getItem('username');
    this.userEmail = <string>localStorage.getItem('useremail');
    this.userImage = <string>localStorage.getItem('userimage');
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
}
