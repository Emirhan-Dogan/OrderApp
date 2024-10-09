import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IProduct } from 'src/app/core/models/IProduct';
import { IStorage } from 'src/app/core/models/IStorage';
import { ProductService } from 'src/app/features/product/service/product.service';
import { StorageService } from 'src/app/features/storage/service/storage.service';

@Component({
  selector: 'app-storage-detail',
  standalone: true,
  imports: [
    DatePipe
  ],
  templateUrl: './storage-detail.component.html',
  styleUrl: './storage-detail.component.scss'
})
export class StorageDetailComponent implements OnInit {
  storageId:number=0;
  storage?:IStorage;
  products:IProduct[]=[];

  constructor(private route:ActivatedRoute, private storageService:StorageService,
    private productService:ProductService,
    private router:Router
  ){}

  ngOnInit(): void {
    // Parametreyi almak için
    this.storageId = parseInt(this.route.snapshot.paramMap.get('id') as string);
    this.storageService.getById(this.storageId).subscribe(
      res=>{
        this.storage=res;
      }
    )

    this.productService.getAllForStorage(this.storageId).subscribe(
      res=>{
        this.products=res;
      }
    )
  }

  productDetail(id: number) {
    this.router.navigate(['/product-detail', id]); // product.id'yi URL parametresi olarak gönderiyoruz
  }
}
