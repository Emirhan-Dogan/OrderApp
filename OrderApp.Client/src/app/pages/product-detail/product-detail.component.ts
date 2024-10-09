import { JsonPipe } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProduct } from 'src/app/core/models/IProduct';
import { ProductService } from 'src/app/features/product/service/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import Swal from 'sweetalert2';
import { MatDialog } from '@angular/material/dialog';
import { ProductUpdateModalComponent } from 'src/app/features/product/modals/product-update-modal/product-update-modal.component';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss',
})
export class ProductDetailComponent {
  productId: number = 0;
  product?: IProduct;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productService: ProductService,
    private dialog:MatDialog,
    private authService:AuthService
  ) {}

  ngOnInit(): void {
    // Parametreyi almak için
    this.productId = parseInt(this.route.snapshot.paramMap.get('id') as string);
    this.productService.getById(this.productId).subscribe((res) => {
      this.product = res;
    });
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  deleteProduct(id: number) {
    Swal.fire({
      title: 'Emin misiniz?',
      text: 'Bu öğe silinecek ve değişiklikler kaydedilecektir!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText: 'İptal',
    }).then((result) => {
      if (result.isConfirmed) {
        this.productService.delete(id).subscribe({
          next: () => {
            this.router.navigate(['/products']);
          },
        });
      }
    });
  }

  updateProduct(id:number){
    const dialogRef=this.dialog.open(ProductUpdateModalComponent, {
      width:'750px',
      data: this.product ,
      disableClose: true
    })
    dialogRef.afterClosed().subscribe({
      next:(res)=>{
        this.productService.getById(this.product!.id).subscribe(
          res=>{
          this.product= res;
        })
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
        this.productService.getById(this.product!.id).subscribe(
          res=>{
          this.product= res;
        })
        Swal.fire({
          title: 'Hata!',
          text: 'İşlem tamamlanamadı.',
          icon: 'error',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
      }}
    )
  }
}
