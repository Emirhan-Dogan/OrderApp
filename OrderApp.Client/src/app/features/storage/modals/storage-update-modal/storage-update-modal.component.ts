import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
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
import { IStorageByUpdateDTO } from 'src/app/core/models/DTOs/IStorageByUpdateDTO';
import { IStorage } from 'src/app/core/models/IStorage';


@Component({
  selector: 'app-storage-update-modal',
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
    CommonModule
  ],
  templateUrl: './storage-update-modal.component.html',
  styleUrl: './storage-update-modal.component.scss',
})
export class StorageUpdateModalComponent implements OnInit{
  storageData:any;
  cities:ICity[]=[];
  showCities:ICity[]=[];
  countries:ICountry[]=[];
  readonly dialogRef = inject(MatDialogRef<StorageUpdateModalComponent>);

  storageForm=this.fb.group({
    name:['',[Validators.required]],
    country:[''],
    city:[''],
    addressDetail:['']
  })

  constructor(private fb:FormBuilder, private cityService:CityService, 
    private countryService:CountryService, private storageService:StorageService,
    @Inject(MAT_DIALOG_DATA) public data: any){
    countryService.getAll().subscribe(
      res=>{
        this.countries= res;
        //console.log(res);
      }
    )
    cityService.getAll().subscribe(
      res=>{
        this.cities=res;
        this.cities.forEach(element => {
          if(element.countryId===parseInt(this.storageData.address.countryId)){
            this.showCities.push(element);
          }
        });
      }
    )
    this.storageData=data;
    
  }
  ngOnInit(): void {
    
    // Form kontrolü için valueChanges dinleyicisi ekleme
    if(this.storageData){
      this.storageForm.patchValue({
        name:this.storageData.name,
        addressDetail:this.storageData.address.addressDetail,
        city:this.storageData.address.cityId,
        country: this.storageData.address.countryId
      })
    }

    this.storageForm.get('country')?.valueChanges.subscribe((selectedCountryId) => {
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
    if(this.storageForm.valid){
      let body:IStorageByUpdateDTO={
        name:this.storageForm.value.name ?? '',
        address:{
          name:this.storageForm.value.name??'',
          lastUpdatedUserId:parseInt(localStorage.getItem('userid')as string),
          dateOfReceipt: new Date,
          status:true,
          addressDetail:this.storageForm.value.addressDetail??'',
          cityId: parseInt(this.storageForm.value.city as string), 
          countryId: parseInt(this.storageForm.value.country as string),
          id:this.storageData.address.id,
          isDeleted:this.storageData.address.isDeleted
        },
        lastUpdatedUserId:parseInt(localStorage.getItem('userid')as string),
        status:true,
        id:this.storageData.id,
        isDeleted:this.storageData.isDeleted
      }
      Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu öğe güncellenecek ve değişiklikler kaydedilecektir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, güncelle!',
        cancelButtonText: 'İptal'
      }).then((result) => {
        if (result.isConfirmed) {
          this.storageService.update(body).subscribe(
            res=>{
              console.log(body);
              this.dialogRef.close(true);
            }
          )
        }
      });
    }
  }
}
