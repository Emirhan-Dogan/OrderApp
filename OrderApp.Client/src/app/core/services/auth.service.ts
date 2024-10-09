import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.development';
import { IUserForLoginDTO } from '../models/IUserForLoginDTO';
import { TokenModel } from '../models/TokenModel';
import { LocalStorageService } from './local-storage.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SharedService } from './shared.service';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userName: string='';
  isLoggin: boolean=false;
  decodedToken: any;
  userToken: string='';
  jwtHelper: JwtHelperService = new JwtHelperService();
  claims: string[]=[];

  constructor(private httpClient: HttpClient, private storageService: LocalStorageService, 
    private router: Router, private sharedService:SharedService) {

    this.setClaims();
  }

  login(loginUser: IUserForLoginDTO) {

    let headers = new HttpHeaders();
    headers = headers.append("Content-Type", "application/json")

    this.httpClient.post<TokenModel>(environment.apiUrl + "/Auth/login", loginUser, { headers: headers }).subscribe({
      next:(data) => {
        if (data.success) {
          
          this.storageService.setToken(data.data.token);
          console.log('Token gönderildi: ' + data.data.token);
          this.storageService.setItem("refreshToken", data.data.refreshToken);
          this.claims = data.data.claims;
    
          localStorage.setItem('username', data.data.user.fullName);
          localStorage.setItem('useremail', data.data.user.email);
          localStorage.setItem('userimage', data.data.user.imagePath);
          localStorage.setItem('userid', data.data.user.userId);
          
          var decode = this.jwtHelper.decodeToken(this.storageService.getToken());
    
          var propUserName = Object.keys(decode).filter(x => x.endsWith("/name"))[0];
          this.userName = decode[propUserName];
          this.sharedService.sendChangeUserNameEvent(this.userName);
    
          this.router.navigateByUrl("/home");
        }
      },
      error:() => {
        // Hata olduğunda SweetAlert2 ile bildirim göster
        Swal.fire({
          title: 'Sunucu Hatası!',
          text: 'Giriş sırasında bir hata oluştu. Lütfen daha sonra tekrar deneyin.',
          icon: 'error',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
        console.error('Giriş hatası:');
      }
  });
    
  }

  resetPassword(forgotPasswordBody:any){
    return this.httpClient.put<any>(environment.apiUrl+'/Auth/forgot-password',forgotPasswordBody)
  }

  getUserName(): string {
    return this.userName;
  }

  setClaims() {

    if ((this.claims == undefined || this.claims.length == 0) && this.storageService.getToken() != null && this.loggedIn() ) {

      this.httpClient.get<string[]>(environment.apiUrl + "/operation-claims/cache").subscribe(data => {
        this.claims =data;
      })

    
      var token = this.storageService.getToken();
      var decode = this.jwtHelper.decodeToken(token);

      var propUserName = Object.keys(decode).filter(x => x.endsWith("/name"))[0];
      this.userName = decode[propUserName];
    }
  }

  logOut() {
    this.storageService.removeToken();
    this.storageService.removeItem("lang")
    this.storageService.removeItem("refreshToken");
    this.claims = [];
  }

  loggedIn(): boolean {

    let isExpired = this.jwtHelper.isTokenExpired(this.storageService.getToken(),-120);
    return !isExpired;
  }

  getCurrentUserId() {
    this.jwtHelper.decodeToken(this.storageService.getToken()).userId;
  }

  claimGuard(claim: string): boolean {
    if(!this.loggedIn())
     this.router.navigate(["/login"]);
    
    var check = this.claims.some(function (item) {
      return item == claim;
    })

    return check;
  }
}
