import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { IOrder } from 'src/app/core/models/IOrder';
import { CustomerService } from 'src/app/features/customer/service/customer.service';
import { OrderModalComponent } from 'src/app/features/order/modals/order-modal/order-modal.component';
import { OrderService } from 'src/app/features/order/service/order.service';
import Swal from 'sweetalert2';
declare var $: any;

@Component({
  selector: 'app-customer-detail',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CustomerDetailComponent implements AfterViewInit, OnInit {
  orderId: number = 0;
  customer: any;
  orders: IOrder[] = [];
  dataTable: any;
  route = inject(ActivatedRoute);
  dialog = inject(MatDialog);
  router = inject(Router);

  constructor(private orderService: OrderService, private customerService: CustomerService) {}

  ngOnInit(): void {
    this.orderId = parseInt(this.route.snapshot.paramMap.get('id') as string);

    // 1. Müşteri bilgilerini al
    this.customerService.getById(this.orderId).subscribe(
      customerRes => {
        this.customer = customerRes;
        // 2. Müşteri bilgilerini aldıktan sonra siparişleri al
        this.loadOrders();
      },
      error => {
        console.error('Müşteri bilgileri alınırken hata oluştu:', error);
      }
    );
  }

  loadOrders(): void {
    this.orderService.getAllForCustomer(this.orderId).subscribe(
      ordersRes => {
        this.orders = ordersRes;
        this.initializeTable(); // Siparişler alındıktan sonra tabloyu başlat
      },
      error => {
        console.error('Siparişler alınırken hata oluştu:', error);
      }
    );
  }

  initializeTable() {
    if (this.dataTable) {
      this.dataTable.destroy();
    }

    this.dataTable = $('#datatable-buttons').DataTable({
      data: this.orders,
      columns: [
        { title: 'Müşteri', data: 'customer.user.email' },
        {
          title: 'Durum',
          data: 'status',
          render: function (data: boolean, _type: any, row: any) {
            return data
              ? '<span class="status-active">Aktif</span>'
              : '<span class="status-inactive">Pasif</span>';
          }
        },
        { title: 'Durum', data: 'deliveryStatus' },
        { title: 'Toplam Fiyat', data: 'totalPrice' },
        {
          title: 'Daha Fazla',
          data: 'id',
          render: function (data: number, _type: any, row: any) {
            return `
              <div class="btn-group" role="group">
                <button class="btn btn-info btn-sm m-1 detail-btn" data-id="${data}">
                  <i class="fas fa-eye"></i> Detay
                </button>
                <button class="btn btn-warning btn-sm m-1 update-btn" data-id="${data}">
                  <i class="fas fa-edit"></i> Düzenle
                </button>
                <button class="btn btn-danger btn-sm m-1 delete-btn" data-id="${data}">
                  <i class="fas fa-trash-alt"></i> Sil
                </button>
              </div>
            `;
          }
        }
      ],
      drawCallback: () => {
        // Detay butonu için olay bağlama
        $('.detail-btn').on('click', (event: any) => {
          const id = $(event.currentTarget).data('id');
          this.viewOrderDetails(id);
        });

        // Düzenle butonu için olay bağlama
        $('.update-btn').on('click', (event: any) => {
          const id = $(event.currentTarget).data('id');
          this.updateOrder(id);
        });

        // Sil butonu için olay bağlama
        $('.delete-btn').on('click', (event: any) => {
          const id = $(event.currentTarget).data('id');
          this.deleteOrder(id);
        });
      },
      responsive: true // Tabloyu responsive yapma
    });
  }

  ngAfterViewInit(): void {
    // Tabloyu başlatmak için ngAfterViewInit kullanılabilir ancak veriler yüklendikten sonra başlatılmalı
    // Bu nedenle initializeTable() metodunu loadOrders içinde çağırıyoruz
  }

  viewOrderDetails(id: number) {}
  updateOrder(id: number) {}
  
  deleteOrder(id: number) {
    Swal.fire({
      title: 'Emin misiniz?',
      text: 'Bu işlem geri alınamaz!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText: 'İptal',
    }).then((result) => {
      if (result.isConfirmed) {
        this.orderService.delete(id).subscribe(() => {
          this.loadOrders(); // Siparişleri yeniden yükle
          Swal.fire({
            title: 'Başarılı!',
            text: 'Kayıt başarıyla silindi.',
            icon: 'success',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        }, (error) => {
          console.error('Silme işlemi sırasında hata oluştu:', error);
          Swal.fire({
            title: 'Hata!',
            text: 'Silme işlemi başarısız oldu.',
            icon: 'error',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        });
      }
    });
  }
  
  orderCreateModal() {
    const dialogRef = this.dialog.open(OrderModalComponent, {
      width: '700px'
    });

    dialogRef.afterClosed().subscribe({
      next: (res) => {
        this.loadOrders(); // Siparişleri yeniden yükle
        if (res) {
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
      error: () => {
        this.loadOrders(); // Siparişleri yeniden yükle
        Swal.fire({
          title: 'Hata!',
          text: 'İşlem tamamlanamadı.',
          icon: 'error',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    });
  }
}
