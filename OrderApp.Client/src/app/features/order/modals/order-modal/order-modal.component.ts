import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { map, Observable, startWith } from 'rxjs';
import { IOrderByCreateDTO } from 'src/app/core/models/DTOs/IOrderByCreateDTO';
import { IProductOrderByCreateDTO } from 'src/app/core/models/DTOs/IProductOrderByCreateDTO';
import { ICustomer } from 'src/app/core/models/ICustomer';
import { IProduct } from 'src/app/core/models/IProduct';
import { CustomerService } from 'src/app/features/customer/service/customer.service';
import { ProductListModalComponent } from 'src/app/features/product/modals/product-list-modal/product-list-modal.component';
import { ProductService } from 'src/app/features/product/service/product.service';
import { deliveryStatus } from 'src/app/shared/constants/deliveryStatus';
import Swal from 'sweetalert2';
import { OrderService } from '../../service/order.service';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
@Component({
  selector: 'app-order-modal',
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
    FormsModule,
    MatAutocompleteModule,
    AsyncPipe,
    CommonModule
  ],
  templateUrl: './order-modal.component.html',
  styleUrl: './order-modal.component.scss',
})
export class OrderModalComponent {
  customers: ICustomer[] = [];
  //filteredCustomers: Observable<any[]>
  products: IProduct[] = [];
  readonly dialogRef = inject(MatDialogRef<OrderModalComponent>);
  deliveryStatus = Object.keys(deliveryStatus).filter((key) =>
    isNaN(Number(key))
  );
  deliveryStatusValues = Object.values(deliveryStatus).filter(
    (value) => !isNaN(Number(value))
  );
  productOrders: any[] = [];
  totalPrice: number = 0;

  orderForm = this.fb.group({
    customer: [''],
    totalPrice: ['0'],
    dateOfReceipt: [''],
    deliveryStatus: [''],
    deliveryNotes: [''],
    products: this.fb.array([]),
  });
  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private productService: ProductService,
    private dialog: MatDialog,
    private orderService: OrderService
  ) {
    customerService.getAll().subscribe((res) => {
      this.customers = res;
    });
    productService.getAll().subscribe((res) => {
      this.products = res;
    });
  }
  // ngOnInit(): void {
  //   this.filteredCustomers = this.orderForm.value.customer!.valueChanges.pipe(
  //     startWith(''),
  //     map(value => this._filter(value || '')),
  //   );
  // }
  // private _filter(value: string) {
  //   const filterValue = value.toLowerCase();
  //   return this.customers.filter(customers => customers.user.email.toLowerCase().includes(filterValue));
  // }

  onProductQuantityChange(newQuantity: any, index: number): void {
    const productCode = this.items.at(index).get('productCode')?.value;
    const productOrder = this.productOrders.find(
      (o) => o.product.productCode === productCode
    );

    if (productOrder) {
      productOrder.count = newQuantity;
      this.calculateTotalPrice();

      console.log(
        'Ürün adedi değişti:',
        newQuantity,
        'Ürün Kodu:',
        productCode
      );
    } else {
      console.log('Ürün bulunamadı:', productCode);
    }
  }

  calculateTotalPrice(): void {
    this.totalPrice = 0;
    this.productOrders.forEach((productOrder) => {
      this.totalPrice +=parseFloat(productOrder.product.price) * parseInt(productOrder.count)
    });
  }

  get items(): FormArray {
    return this.orderForm.get('products') as FormArray;
  }

  createItem(productCode: string): FormGroup {
    return this.fb.group({
      productCode: [productCode, Validators.required],
      productQuantity: ['1', [Validators.required, Validators.min(1)]],
    });
  }

  addItem(): void {
    this.dialog
      .open(ProductListModalComponent, {
        width: '750px',
      })
      .afterClosed()
      .subscribe((res) => {
        let product = this.products.find((p) => p.id == res);
        this.productOrders.forEach((productOrder) => {
          if (productOrder.product.productCode === product?.productCode) {
            Swal.fire({
              title: 'Hata',
              text: 'Bu öğe zaten sepetinizde bulunmaktadır.',
              icon: 'error',
            });
            return;
          }
        });
        this.productOrders.push({ product: product, count: 1 });
        this.calculateTotalPrice();
        this.items.push(this.createItem(product!.productCode));
        (this.items as FormArray).controls.forEach(
          (control: AbstractControl, index: number) => {
            if (control instanceof FormGroup) {
              control.get('productQuantity')?.valueChanges.subscribe((value) => {
                this.onProductQuantityChange(value, index);
              });
            }
          }
        );
      });
  }
  removeProduct(index: number): void {
    const removedProductCode = this.items.at(index).get('productCode')?.value;
    this.items.removeAt(index);
    this.productOrders = this.productOrders.filter(
      (order) => order.product.productCode !== removedProductCode
    );
    if (this.productOrders.length === 0) {
      this.totalPrice = 0;
    } else {
      this.calculateTotalPrice();
    }
  }

  onSubmit() {
    if (this.orderForm.valid) {
      let createdUserId = parseInt(localStorage.getItem('userid') as string);

      const body: IOrderByCreateDTO = {
        customerId: parseInt(this.orderForm.value.customer as string),
        status: true,
        totalPrice: this.totalPrice,
        deliveryNotes: this.orderForm.value.deliveryNotes as string,
        createdUserId: createdUserId,
        dateOfReceipt: new Date(
          this.orderForm.value.dateOfReceipt || new Date()
        ),
        deliveryStatus: this.orderForm.value.deliveryStatus ?? '',
        productOrders: [],
      };
      this.productOrders.forEach((element) => {
        let productOrder: IProductOrderByCreateDTO = {
          createdUserId: createdUserId,
          piece: element.count,
          status: true,
          unitInPrice: element.product.price,
          productId: element.product.id,
        };
        body.productOrders.push(productOrder);
      });

      Swal.fire({
        title: 'Emin misiniz?',
        text: 'Bu öğe eklenecek ve değişiklikler kaydedilecektir!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Evet, ekle!',
        cancelButtonText: 'İptal',
      }).then((result) => {
        if (result.isConfirmed) {
          this.orderService.add(body).subscribe({
            next: (res) => {
              console.log('Modal kapanıyor, değer true');
              this.dialogRef.close(true);
            },
            error: (err) => {
              console.log('Hata objesi:', JSON.stringify(err));

              Swal.fire({
                icon: 'error',
                title: 'Hata!',
                text: 'Stok sayısını aştınız.',
                confirmButtonText: 'OK'
              });
              console.error('Hata:', err);
            },
          });
        } else if (result.isDismissed) {
          this.dialogRef.close(false);
        }
      });
    }
  }
}
