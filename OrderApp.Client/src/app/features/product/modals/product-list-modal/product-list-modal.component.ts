import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ProductService } from '../../service/product.service';
import { IProduct } from 'src/app/core/models/IProduct';

@Component({
  selector: 'app-product-list-modal',
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
    CommonModule
  ],
  templateUrl: './product-list-modal.component.html',
  styleUrl: './product-list-modal.component.scss'
})
export class ProductListModalComponent {
  readonly dialogRef = inject(MatDialogRef<ProductListModalComponent>);
  products:IProduct[]=[];

  constructor(private productService:ProductService){
    productService.getAll().subscribe(
      res=>{
        this.products= res
      }
    )
  }
  addToBasket(id:number){
    this.dialogRef.close(id);
  }
}
