import { AfterViewInit, Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { ProductModalComponent } from 'src/app/features/product/modals/product-modal/product-modal.component';
import { Router } from '@angular/router';
import { ProductService } from 'src/app/features/product/service/product.service';
import { IProduct } from 'src/app/core/models/IProduct';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements AfterViewInit{
  dialog = inject(MatDialog);
  router = inject(Router);
  public products: IProduct[] = [];
  page:number=1;
  hasAfterProducts=true;
  hasBeforeProducts=false;
  constructor(private productService: ProductService,
    private authService:AuthService
  ) {
    
  }
  ngAfterViewInit(): void {
    this.initializeProducts();
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  loadAfterPage(){
    this.page++;
    this.initializeProducts();
    this.hasBeforeProducts=true;
    if(10>this.products.length){
      this.hasAfterProducts=false
      this.triggerButtonAnimation('next');
    }
  }

  loadBeforePage(){
    this.page--;
    this.initializeProducts();
    this.hasAfterProducts=true;
    if(this.page==1){
      this.hasBeforeProducts=false
      this.triggerButtonAnimation('previous');
    }
  }
  
  initializeProducts(){
      this.productService.getAllPagination(this.page).subscribe(
        (res) => {
          if(res.length===0){
            this.page--;
            this.triggerButtonAnimation('next');
            this.initializeProducts();
            this.hasAfterProducts=false;
          }else{
          this.products = res;
          }
        }
      );
  }

  // Buton animasyonunu başlat
  private triggerButtonAnimation(buttonType: 'previous' | 'next') {
    const button = document.querySelector(`.load-more-btn.${buttonType}`);
    if (button) {
      button.classList.add('animate-button');
      setTimeout(() => {
        button.classList.remove('animate-button');
      }, 250);
    }
  }

  productDetail(id: any) {
    this.router.navigate(['/product-detail', id]); // product.id'yi URL parametresi olarak gönderiyoruz
  }

  productCreateModal() {
    const dialogRef = this.dialog.open(ProductModalComponent, {
      width: '500px'
    });
  
    dialogRef.afterClosed().subscribe(result => {
        this.initializeProducts();
    });
  }
  
}
