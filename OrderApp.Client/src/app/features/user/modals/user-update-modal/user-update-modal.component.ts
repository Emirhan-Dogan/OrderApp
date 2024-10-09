import { Component, Inject, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
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
import Swal from 'sweetalert2';
import { UserService } from '../../service/user.service';
import { Router } from '@angular/router';
import { ImageService } from 'src/app/core/services/image.service';
import { IUser } from 'src/app/core/models/IUser';
import { IUserByCreateDTO } from 'src/app/core/models/DTOs/IUserByCreateDTO';
import { ICustomerByUpdateDTO } from 'src/app/core/models/DTOs/ICustomerByUpdateDTO';
import { IUserByUpdateDTO } from 'src/app/core/models/DTOs/IUserByUpdateDTO';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-update-modal',
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
    MatSelectModule,
    CommonModule,
  ],
  templateUrl: './user-update-modal.component.html',
  styleUrl: './user-update-modal.component.scss',
})
export class UserUpdateModalComponent implements OnInit {
  imagePath: string = '';
  userData: any;

  readonly dialogRef = inject(MatDialogRef<UserUpdateModalComponent>);
  userForm = this.fb.group({
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    mobilePhones: [''],
    gender: ['2'],
    birthDate: [''],
    addressDetail: [''],
  });

  constructor(
    private fb: FormBuilder,
    private imageService: ImageService,
    private userService: UserService,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.userData = data;
  }

  ngOnInit(): void {
    if (this.userData) {
      // Formu güncellerken:
      this.userForm.patchValue({
        fullName: this.userData.fullName,
        email: this.userData.email,
        mobilePhones: this.userData.mobilePhones,
        addressDetail: this.userData.address,
        gender: this.userData.gender.toString(), // String formatında
        birthDate: this.userData.birthDate 
          ? new Date(this.userData.birthDate).toLocaleDateString('en-CA') 
          : '' 
      });

      this.imagePath = this.userData.imagePath;
    }
  }

  onSubmit() {
    if (this.userForm.valid) {
      const currentDate = new Date().toISOString();

      const body: IUserByUpdateDTO = {
        userId: this.userData.userId,
        fullName: this.userForm.value.fullName ?? '',
        email: this.userForm.value.email ?? '',
        imagePath: this.imagePath??'',
        notes: '',
        password: '',
        status: true,
        mobilePhones: this.userForm.value.mobilePhones ?? '',
        address: this.userForm.value.addressDetail ?? '',
        birthDate: this.userForm.value.birthDate? new Date(this.userForm.value.birthDate as string) : new Date(), 
        gender: parseInt(this.userForm.value.gender || '0'),
        citizenId: 0,
        recordDate: currentDate,
        updateContactDate: currentDate,
      };

      Swal.fire({
        title: 'Emin misiniz?',
        text: 'Bu öğe güncellenecek ve değişiklikler kaydedilecektir!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, ekle!',
        cancelButtonText: 'İptal',
      }).then((result) => {
        if (result.isConfirmed) {
          this.userService.update(body).subscribe({
            next: (res) => {
              console.log('Modal kapanıyor, değer true');
              this.dialogRef.close(true); // Modal'ı şimdi kapat
            },
            error: (err) => {
              console.error('Hata:', err);
            },
          });
        }
      });
    } else {
      Swal.fire(
        'Geçersiz Form!',
        'Lütfen gerekli alanları doldurun.',
        'warning'
      );
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

    return this.imageService
      .uploadImage(file)
      .pipe(catchError(this.handleError));
  }
  private handleError(error: any): Observable<never> {
    console.error('Bir hata oluştu:', error);
    // Burada hatayı yeniden fırlatabilir veya daha ileri işleme yapabilirsiniz
    throw error;
  }
}
