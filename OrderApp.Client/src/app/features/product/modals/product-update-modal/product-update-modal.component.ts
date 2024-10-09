import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogModule, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { IProductByCreateDTO } from 'src/app/core/models/DTOs/IProductByCreateDTO';
import { ICategory } from 'src/app/core/models/ICategory';
import { IColor } from 'src/app/core/models/IColor';
import { IStorage } from 'src/app/core/models/IStorage';
import { CategoryService } from 'src/app/core/services/category.service';
import { ColorService } from 'src/app/core/services/color.service';
import { ImageService } from 'src/app/core/services/image.service';
import { StorageService } from 'src/app/features/storage/service/storage.service';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../service/product.service';
import Swal from 'sweetalert2';
import { catchError, Observable } from 'rxjs';
import { ColorModalComponent } from '../color-modal/color-modal.component';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { IProductByUpdateDTO } from 'src/app/core/models/DTOs/IProductByUpdateDTO';
import { sizeCode } from 'src/app/shared/constants/productSize';

@Component({
  selector: 'app-product-update-modal',
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
    MatSelectModule,
    MatDialogModule,
    CommonModule
  ],
  templateUrl: './product-update-modal.component.html',
  styleUrls: ['./product-update-modal.component.scss']
})
export class ProductUpdateModalComponent implements OnInit {
  productData: any;  
  imagePath: string = '';
  colors: IColor[] = [];
  categories: ICategory[] = [];
  storages: IStorage[] = [];
  sizeCodes = Object.keys(sizeCode).filter(key => isNaN(Number(key)));
  sizeCodeValues = Object.values(sizeCode).filter(value => !isNaN(Number(value))); 


  productForm = this.fb.group({
    name: ['', Validators.required],
    productCode: ['', Validators.required],
    description: [''],
    price: ['', Validators.required],
    sizeCode: [''],
    stockCount: [''],
    category: [''],
    storage: [''],
    color: ['']
  });

  constructor(
    private imageService: ImageService,
    private fb: FormBuilder,
    private colorService: ColorService,
    private categoryService: CategoryService,
    private storageService: StorageService,
    private productService: ProductService,
    private dialog:MatDialog,
    public dialogRef: MatDialogRef<ProductUpdateModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.productData = data; 
    this.colorService.getAll().subscribe(res => this.colors = res);
    this.categoryService.getAll().subscribe(res => this.categories = res);
    this.storageService.getAll().subscribe(res => this.storages = res);
  }

  ngOnInit(): void {
    if (this.productData) {
      this.productForm.patchValue({
        name: this.productData.name,
        productCode: this.productData.productCode,
        description: this.productData.description,
        price: this.productData.price,
        sizeCode: sizeCode[this.productData.sizeCode],
        stockCount: this.productData.stockCount,
        category: this.productData.categoryId,
        storage: this.productData.storageId,
        color: this.productData.colorId
      });
      this.imagePath=this.productData.imagePath;
    }
  }

  onSubmit() {
    if (this.productForm.valid) {
      let body: IProductByUpdateDTO = {
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
        lastUpdatedUserId: parseInt(localStorage.getItem('userid') as string),
        id:this.productData.id,
        isDeleted:false
      };
      
      Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu öğe güncellenecek ve değişiklikler kaydedilecektir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, Güncelle!',
        cancelButtonText: 'İptal'
      }).then(result => {
        if (result.isConfirmed) {
          this.productService.update(body).subscribe({
            next:()=>{
              this.dialogRef.close(true);
            },
            error:()=>{
              this.dialogRef.close(true);
            }
          });
        }
      });
    }
  }
  
  openColorDialog() {
    const dialogRef = this.dialog.open(ColorModalComponent, {
      width: '750px'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.colorService.getAll().subscribe(res => this.colors = res);
    });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.uploadImage(file).subscribe(
        response => {
          this.imagePath = response.body.path;
        },
        error => console.error('Dosya yüklenirken bir hata oluştu.', error)
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
    throw error;
  }
}
