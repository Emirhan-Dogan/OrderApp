import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogClose, MatDialogContent, MatDialogModule, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { IColorByCreateDTO } from 'src/app/core/models/DTOs/IColorByCreateDTO';
import { ColorService } from 'src/app/core/services/color.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-color-modal',
  standalone: true,
  imports:[
    MatButtonModule,
    MatDialogActions,
    MatDialogClose,
    MatDialogTitle,
    MatDialogContent,
    MatDialogModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule
  ],
  templateUrl: './color-modal.component.html',
  styleUrls: ['./color-modal.component.scss'],
})
export class ColorModalComponent {
  readonly dialogRef = inject(MatDialogRef<ColorModalComponent>);
  
  colorForm = this.fb.group({
    colorName: ['', Validators.required],
    colorCode: ['#ffffff', Validators.required] // Initial value is set to white
  });

  constructor(private fb: FormBuilder, private colorService: ColorService) {}

  onSubmit() {
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
        const body: IColorByCreateDTO = {
          colorName: this.colorForm.value.colorName ?? '',
          colorCode: this.colorForm.value.colorCode ?? '#ffffff',
          createdUserId: parseInt(localStorage.getItem('userid') as string),
          status: true,
        };
        
        this.colorService.add(body).subscribe(
          res => {
            console.log('Color created:', body);
            this.dialogRef.close(true); // Başarıyla kapat
          },
          error => {
            console.error('Error creating color:', error);
            this.dialogRef.close(true);
          }
        );
      }
    });
  }
}
