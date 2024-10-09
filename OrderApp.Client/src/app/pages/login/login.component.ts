import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { IUserForLoginDTO } from 'src/app/core/models/IUserForLoginDTO';
import { AuthService } from 'src/app/core/services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule, CommonModule, MatInputModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm= this.fb.group({
    email:['',[Validators.required, Validators.email]],
    password:['',Validators.required]
  });

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  forgotPassword(){
    this.router.navigate(['/login'])
  }

  login() {
    if(this.loginForm.valid){
      const loginBody:IUserForLoginDTO={
        email: (this.loginForm.value.email) as string,
        password: (this.loginForm.value.password) as string,
      }
      this.authService.login(loginBody)
      console.log('Calıştı..'+ loginBody);
      
    }
  }
}
