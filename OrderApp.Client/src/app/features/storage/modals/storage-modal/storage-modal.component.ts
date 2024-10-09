import { CommonModule } from '@angular/common';
import { Component, Inject, inject } from '@angular/core';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
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
import { CityService } from 'src/app/core/services/city.service';
import { CountryService } from 'src/app/core/services/country.service';
import { StorageService } from '../../service/storage.service';
import { ICity } from 'src/app/core/models/ICity';
import { ICountry } from 'src/app/core/models/ICountry';
import { IStorageByCreateDTO } from 'src/app/core/models/DTOs/IStorageByCreateDTO';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-storage-modal',
  standalone: true,
  imports: [
    MatButtonModule,
    MatDialogActions,
    MatDialogClose,
    MatDialogTitle,
    MatDialogContent,
    ReactiveFormsModule,
    MatInputModule,
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    CommonModule,
  ],
  templateUrl: './storage-modal.component.html',
  styleUrl: './storage-modal.component.scss',
})
export class StorageModalComponent {
  cities: ICity[] = [];
  showCities: ICity[] = [];
  countries: ICountry[] = [];
  readonly dialogRef = inject(MatDialogRef<StorageModalComponent>);

  storageForm = this.fb.group({
    name: ['', [Validators.required]],
    country: [''],
    city: [''],
    addressDetail: [''],
  });

  constructor(
    private fb: FormBuilder,
    private cityService: CityService,
    private countryService: CountryService,
    private storageService: StorageService
  ) {
    countryService.getAll().subscribe((res) => {
      this.countries = res;
      //console.log(res);
    });
    cityService.getAll().subscribe((res) => {
      this.cities = res;
      //console.log(res);
    });
  }
  ngOnInit(): void {
    // Form kontrolü için valueChanges dinleyicisi ekleme
    this.storageForm
      .get('country')
      ?.valueChanges.subscribe((selectedCountryId) => {
        console.log('Seçilen ülke ID:', selectedCountryId);
        this.showCities=[];
        this.cities.forEach((element) => {
          if (element.countryId == parseInt(selectedCountryId as string)) {
            this.showCities.push(element);
          }
        });
      });
  }

  onSubmit() {
    if (this.storageForm.valid) {
      let body: IStorageByCreateDTO = {
        name: this.storageForm.value.name ?? '',
        address: {
          name: this.storageForm.value.name ?? '',
          createdUserId: parseInt(localStorage.getItem('userid') as string),
          dateOfReceipt: new Date(),
          status: true,
          addressDetail: this.storageForm.value.addressDetail ?? '',
          cityId: parseInt(this.storageForm.value.city as string),
          countryId: parseInt(this.storageForm.value.country as string),
        },
        createdUserId: parseInt(localStorage.getItem('userid') as string),
        status: true
      };
  
      // SweetAlert sorusu çıktığında modalı kapatma!
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
          // Onay aldıktan sonra API çağrısı yap
          this.storageService.add(body).subscribe({
            next: (res) => {
              console.log('Modal kapanıyor, değer true');
              this.dialogRef.close(true); // Modal'ı şimdi kapat
            },
            error: (err) => {
              console.error('Hata:', err);
            }
          });
        } else if (result.isDismissed) {
          // Eğer SweetAlert iptal edilirse modalı false ile kapat
          this.dialogRef.close(false);
        }
      });
    }
  }
  
}
