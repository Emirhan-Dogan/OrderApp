import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialog,
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
import { IProductByCreateDTO } from 'src/app/core/models/DTOs/IProductByCreateDTO';
import { ICategory } from 'src/app/core/models/ICategory';
import { IColor } from 'src/app/core/models/IColor';
import { IStorage } from 'src/app/core/models/IStorage';
import { CategoryService } from 'src/app/core/services/category.service';
import { ColorService } from 'src/app/core/services/color.service';
import { ImageService } from 'src/app/core/services/image.service';
import { StorageService } from 'src/app/features/storage/service/storage.service';
import { ColorModalComponent } from '../color-modal/color-modal.component';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../service/product.service';
import Swal from 'sweetalert2';
import { sizeCode } from 'src/app/shared/constants/productSize';

@Component({
  selector: 'app-product-modal',
  standalone: true,
  imports: [
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
    FormsModule,
    CommonModule
  ],
  templateUrl: './product-modal.component.html',
  styleUrl: './product-modal.component.scss'
})
export class ProductModalComponent {
  imagePath:string='';
  colors:IColor[]=[];
  categories:ICategory[]=[];
  storages:IStorage[]=[];
  sizeCodes = Object.keys(sizeCode).filter(key => isNaN(Number(key)));
  sizeCodeValues = Object.values(sizeCode).filter(value => !isNaN(Number(value))); 

  readonly dialogRef = inject(MatDialogRef<ProductModalComponent>);
  productForm=this.fb.group({
    name:['',Validators.required],
    productCode:['',Validators.required],
    description:[''],
    price:['0',[Validators.required]],
    sizeCode:[''],
    stockCount:['0'],
    category:[''],
    storage:[''],
    color:['']
  });
  constructor(private imageService:ImageService, private fb:FormBuilder,
    private colorService:ColorService, private categoryService:CategoryService,
    private storageService:StorageService, private dialog:MatDialog,
    private productService:ProductService
  ){
    this.colorService.getAll().subscribe(
      res=>{
        this.colors=res;
      }
    )
    this.categoryService.getAll().subscribe(
      res=>{
        this.categories=res;
      }
    )
    this.storageService.getAll().subscribe(
      res=>{
        this.storages=res;
      }
    )
  }

  onSubmit() {
    if (this.productForm.valid) {
      const body: IProductByCreateDTO = {
        name: this.productForm.value.name ?? '',
        description: this.productForm.value.description ?? '',
        imagePath: this.imagePath,
        price: parseFloat(this.productForm.value.price as string),
        stockCount: parseInt(this.productForm.value.stockCount as string),
        sizeCode: this.productForm.value.sizeCode ?? '',
        productCode: this.productForm.value.productCode ?? '',
        status: true,
        categoryId: parseInt(this.productForm.value.category as string),
        colorId: parseInt(this.productForm.value.color as string),
        storageId: parseInt(this.productForm.value.storage as string),
        createdUserId: parseInt(localStorage.getItem('userid') as string)
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
          this.productService.add(body).subscribe(
            res => {
              this.dialogRef.close(true); // Modal kapatılıyor
            },
            error => {
              console.error('Ürün eklenirken hata:', error);
              this.dialogRef.close(false); // Başarısız durumda modalı kapat
            }
          );
        }
      }).catch(error => {
        console.error('SweetAlert hatası:', error);
      });
    }
  }  
  
  openColorDialog() {
    const dialogRef = this.dialog.open(ColorModalComponent, {
      width: '500px'
    });
  
    dialogRef.afterClosed().subscribe(result => {
      console.log('Dialog kapandı. Sonuç:', result);
      this.colorService.getAll().subscribe(
        res=>{
          this.colors=res;
        }
      )
    });
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
