import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogModule,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { catchError, Observable } from 'rxjs';
import { IUserByCreateDTO } from 'src/app/core/models/DTOs/IUserByCreateDTO';
import { IUser } from 'src/app/core/models/IUser';
import { ImageService } from 'src/app/core/services/image.service';
import { UserService } from '../../service/user.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-modal',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogActions,
    MatDialogClose,
    MatDialogTitle,
    MatDialogContent,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatDialogModule,
    MatSelectModule
  ],
  templateUrl: './user-modal.component.html',
  styleUrl: './user-modal.component.scss'
})
export class UserModalComponent {
  imagePath:string="";

  readonly dialogRef = inject(MatDialogRef<UserModalComponent>);
  userForm=this.fb.group({
    fullName:['',Validators.required],
    email:['',[Validators.required, Validators.email]],
    mobilePhones:[''],
    gender:['2'],
    birthDate:[''],
    addressDetail:['']
  });

  constructor(private fb:FormBuilder, private imageService:ImageService,
    private userService:UserService, private router:Router
  ){

  }

  onSubmit(){
    if(this.userForm.valid){
      const currentDate = new Date().toISOString();
      const body: IUserByCreateDTO = {
        fullName: this.userForm.value.fullName ?? '',
        email: this.userForm.value.email ?? '',
        imagePath: this.imagePath,
        gender: parseInt(this.userForm.value.gender as string),
        notes: '',
        password: '',
        status: true,
        mobilePhones: this.userForm.value.mobilePhones ?? '',
        address: this.userForm.value.addressDetail ?? '',  // Eğer adres formda varsa eklenmesi mantıklı olur
        birthDate: this.userForm.value.birthDate as string,
        citizenId: 0,  // Eğer backend'de farklı bir ID kullanılması gerekiyorsa buna dikkat edilmeli
        recordDate: currentDate,
        updateContactDate: currentDate
      };
  
      Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu öğe eklenecek ve değişiklikler kaydedilecektir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, ekle!',
        cancelButtonText: 'İptal'
      }).then((result) => {
        if (result.isConfirmed) {
          this.userService.add(body).subscribe({
            next: (res) => {
              console.log('Modal kapanıyor, değer true');
              this.dialogRef.close(true); // Modal'ı şimdi kapat
            },
            error: (err) => {
              console.error('Hata:', err);
            }
          });
        }
      });
    } else {
      Swal.fire('Geçersiz Form!', 'Lütfen gerekli alanları doldurun.', 'warning');
    }
  }
  

  // Dosya seçildiğinde çalışacak olan fonksiyon
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];

    if (file) {
      this.uploadImage(file).subscribe(
        (response) => {
          this.imagePath = response.body.path;
          
          
        },
        (error) => {
          console.error('Dosya yüklenirken bir hata oluştu.', error);
        }
      );
    }
  }

  uploadImage(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.imageService.uploadImage(file).pipe(
      catchError(this.handleError)
    );
  }
  private handleError(error: any): Observable<never> {
    console.error('Bir hata oluştu:', error);
    // Burada hatayı yeniden fırlatabilir veya daha ileri işleme yapabilirsiniz
    throw error;
  }
}
