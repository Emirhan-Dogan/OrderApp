import { AfterViewInit, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogModule,
  MatDialogRef,
  MatDialogTitle,} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { catchError, Observable } from 'rxjs';
import { ICustomerByCreateDTO } from 'src/app/core/models/DTOs/ICustomerByCreateDTO';
import { ICity } from 'src/app/core/models/ICity';
import { ICountry } from 'src/app/core/models/ICountry';
import { CityService } from 'src/app/core/services/city.service';
import { CountryService } from 'src/app/core/services/country.service';
import { ImageService } from 'src/app/core/services/image.service';
import { CustomerService } from '../../service/customer.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-customer-modal',
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
    MatSelectModule
  ],
  templateUrl: './customer-modal.component.html',
  styleUrl: './customer-modal.component.scss'
})
export class CustomerModalComponent implements OnInit {
  cities:ICity[]=[];
  showCities:ICity[]=[];
  countries:ICountry[]=[];
  imagePath:string="";

  readonly dialogRef = inject(MatDialogRef<CustomerModalComponent>);
  customerForm= this.fb.group({
    fullName:['',[Validators.required]],
    customerCode:['',[Validators.required]],
    mobilePhones:['',[Validators.required]],
    email:['',[Validators.required, Validators.email]],
    birthDate:[Date.now()],
    gender:['2'],
    country:[''],
    city:[''],
    addressDetail:[''],
  })

  constructor(private fb:FormBuilder, private cityService:CityService, private countryService:CountryService, private imageService:ImageService, private customerService:CustomerService){
    countryService.getAll().subscribe(
      res=>{
        this.countries= res;
        //console.log(res);
      }
    )
    cityService.getAll().subscribe(
      res=>{
        this.cities=res;
        //console.log(res);
      }
    )
  }
  ngOnInit(): void {
    // Form kontrolü için valueChanges dinleyicisi ekleme
    this.customerForm.get('country')?.valueChanges.subscribe((selectedCountryId) => {
      console.log('Seçilen ülke ID:', selectedCountryId);
      this.showCities=[];
      this.cities.forEach(element => {
        if(element.countryId==parseInt(selectedCountryId as string)){
          this.showCities.push(element);
        }
      });
    });
  }

  onSubmit(){
    if (this.customerForm.valid) {
      const body: ICustomerByCreateDTO = {
        fullName: this.customerForm.value.fullName ?? '', 
        email: this.customerForm.value.email ?? '', 
        gender: parseInt(this.customerForm.value.gender || '0'), 
        birthDate: new Date(this.customerForm.value.birthDate || new Date()), 
        customerCode: this.customerForm.value.customerCode ?? '', 
        mobilePhones: this.customerForm.value.mobilePhones ?? '', 
        status: true, 
        imagePath: this.imagePath ?? '', 
        createdUserId: parseInt(localStorage.getItem('userid')as string), 
        address: {
          cityId: parseInt(this.customerForm.value.city as string), 
          countryId: parseInt(this.customerForm.value.country as string), 
          addressDetail: this.customerForm.value.addressDetail ?? '', 
          createdUserId: parseInt(localStorage.getItem('userid')as string), 
          name: this.customerForm.value.fullName ?? '', 
          status: true, 
          dateOfReceipt: new Date(), 
        },
        notes:'',
        citizenId:0
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
          this.customerService.add(body).subscribe({
            next: (res) => {
              console.log('Modal kapanıyor, değer true');
              this.dialogRef.close(true); // Modal'ı şimdi kapat
            },
            error: (err) => {
              console.error('Hata:', err);
            }
          })
        }else if (result.isDismissed) {
          // Eğer SweetAlert iptal edilirse modalı false ile kapat
          this.dialogRef.close(false);
        }
      });
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
