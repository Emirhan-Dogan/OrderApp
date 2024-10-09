import { AfterViewInit, Component, inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { IUser } from 'src/app/core/models/IUser';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserModalComponent } from 'src/app/features/user/modals/user-modal/user-modal.component';
import { UserUpdateModalComponent } from 'src/app/features/user/modals/user-update-modal/user-update-modal.component';
import { UserService } from 'src/app/features/user/service/user.service';
import Swal from 'sweetalert2';
declare var $:any;

@Component({
  selector: 'app-user-claims',
  standalone: true,
  imports: [],
  templateUrl: './user-claims.component.html',
  styleUrl: './user-claims.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class UserClaimsComponent implements AfterViewInit {
  dialog = inject(MatDialog);
  router = inject(Router);
  users:IUser[]=[];
  dataTable:any;
  constructor(private userService:UserService, private authService:AuthService) {
  }
  ngAfterViewInit(): void {
    this.userService.getAll().subscribe(
      (res)=>{
        console.log(res);
        this.users = res;
        this.initializeTable()
      }
    )
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  initializeTable() {
    this.userService.getAll().subscribe((res) => {
      console.log(res);
      this.users = res;  // Kullanıcı verilerini al
  
      if (this.dataTable) {
        this.dataTable.destroy(); // Mevcut tabloyu yok et
      }
  
      this.dataTable = $('#datatable-buttons').DataTable({
        data: this.users,  // users verisini tabloya ekle
        columns: [
          { title: 'İsim', data: 'fullName' },
          { title: 'Email', data: 'email' },
          {
            title: 'Durum',
            data: 'status',
            render: function (data: boolean) {
              return data
                ? '<span class="status-active">Aktif</span>'
                : '<span class="status-inactive">Pasif</span>';
            },
          },
          {
            title: 'Daha Fazla',
            data: 'userId',
            render: function (data: number) {
              return `
                <div class="btn-group" role="group">
                <button class="btn btn-primary btn-sm m-1 reset-btn" data-id="${data}">
                    <i class="fas fa-key"></i> Şifre Sıfırla
                  </button>
                  <button class="btn btn-info btn-sm m-1 detail-btn" data-id="${data}">
                    <i class="fas fa-eye"></i> Yetkilendir
                  </button>
                  <button class="btn btn-warning btn-sm m-1 update-btn" data-id="${data}">
                    <i class="fas fa-edit"></i> Düzenle
                  </button>
                  <button class="btn btn-danger btn-sm m-1 delete-btn" data-id="${data}">
                    <i class="fas fa-trash-alt"></i> Sil
                  </button>
                </div>
              `;
            },
          },
        ],
        drawCallback: () => {
          $('.reset-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('ForgotPasswordCommand')){
              this.resetPassword(id); 
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });

          // Detay butonu için olay bağlama
          $('.detail-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('CreateUserGroupCommand')){
              this.authorizeUser(id); 
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
  
          // Düzenle butonu için olay bağlama
          $('.update-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('CreateUserCommand')){
              this.updateUser(id);
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
  
          // Sil butonu için olay bağlama
          $('.delete-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('DeleteUserCommand')){
              this.deleteUser(id); 
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
        },
      });
    });
  }
  resetPassword(id:number){
    console.log(id);
    
    this.userService.getById(id).subscribe({
      next:(res)=>{
        this.authService.resetPassword({
          email: res.email,
          fullName:res.fullName
        }).subscribe({
          next:()=>{
            Swal.fire({
              title: 'Başarılı!',
              text: 'Şifre Sıfırlandı Maili Kontrol ediniz.',
              icon: 'success',
              confirmButtonText: 'Tamam'
            });    
          },
          error:()=>{
            Swal.fire({
              title: 'Hata!',
              text: 'Bir hata oluştu.',
              icon: 'warning',
              confirmButtonText: 'Tamam'
            });    
          }
        })
      }
    })

    
  }
  
  deleteUser(id:number){
    Swal.fire({
      title: 'Emin misiniz?',
      text: "Bu işlem geri alınamaz!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText: 'İptal'
    }).then((result) => {
      if (result.isConfirmed) {
        // Kullanıcı onay verdiyse silme işlemini gerçekleştir
        this.userService.delete(id).subscribe(() => {
          // Veriyi sildikten sonra tabloyu yeniden yükle
          this.initializeTable(); // Burada tabloyu yeniden başlatın
          Swal.fire({
            title: 'Başarılı!',
            text: 'Kayıt başarıyla silindi.',
            icon: 'success',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        }, (error) => {
          console.error('Silme işlemi sırasında hata oluştu:', error);
          Swal.fire({
            title: 'Hata!',
            text: 'Silme işlemi başarısız oldu.',
            icon: 'error',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        });
      }
    });
  }

  updateUser(id:number){
    const user = this.users.find((s) => s.userId === id);

    if (user) {
      this.dialog
        .open(UserUpdateModalComponent, {
          width: '500px',
          data: user,
          disableClose: true
        })
        .afterClosed()
        .subscribe({
          next:(res)=>{
            this.initializeTable();
            if(res){
              Swal.fire({
                title: 'Başarılı!',
                text: 'İşlem başarılı bir şekilde tamamlandı.',
                icon: 'success',
                position: 'top-end',
                showConfirmButton: false,
                timer: 1500,
              });
            }
          },
          error:()=>{
            this.initializeTable();
            Swal.fire({
              title: 'Hata!',
              text: 'İşlem tamamlanamadı.',
              icon: 'error',
              position: 'top-end',
              showConfirmButton: false,
              timer: 1500,
            });
          }
        });
    } else {
      console.error('Storage not found');
    }
  }
  authorizeUser(id:number){
    this.router.navigate(['/user-claims-detail', id]);
  }

  userCreateModal(){
    this.dialog.open(UserModalComponent, {
      width: '700px'
    }).afterClosed().subscribe(
      (res) => {
        if(res){
          this.initializeTable(); // Burada tabloyu yeniden başlatın
        Swal.fire({
          title: 'Başarılı!',
          text: 'Kayıt başarıyla eklendi.',
          icon: 'success',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
        }
      }, (error) => {
        console.error('Ekleme işlemi sırasında hata oluştu:', error);
        Swal.fire({
          title: 'Hata!',
          text: 'Ekleme işlemi başarısız oldu.',
          icon: 'error',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
      });
  }
}
