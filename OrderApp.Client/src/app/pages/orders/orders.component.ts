import { AfterViewInit, Component, inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { IOrder } from 'src/app/core/models/IOrder';
import { AuthService } from 'src/app/core/services/auth.service';
import { OrderModalComponent } from 'src/app/features/order/modals/order-modal/order-modal.component';
import { OrderService } from 'src/app/features/order/service/order.service';
import Swal from 'sweetalert2';
declare var $:any;

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
  encapsulation:ViewEncapsulation.None
})
export class OrdersComponent implements AfterViewInit{
  dialog = inject(MatDialog);
  router= inject(Router);
  orders: IOrder[] = [];
  dataTable:any;
  constructor(private orderService: OrderService,
    private authService:AuthService
  ) {
    this.orderService.getAll().subscribe((res) => {
      console.log(res);
      this.orders = res;
    });
    
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  initializeTable() {

    this.orderService.getAll().subscribe((res) => {
      console.log(res);
      this.orders = res;
  
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
                  <button class="btn btn-danger btn-sm m-1 delete-btn" data-id="${data}">
                    <i class="fas fa-trash-alt"></i> Sil
                  </button>
                </div>
              `;
            }
          }
        ],
        drawCallback: () => {
          $('.delete-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('DeleteOrderCommand')){
              this.deleteOrder(id);
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
        },
        responsive: true // Tabloyu responsive yapma
      });
    });
  }
  
  ngAfterViewInit(): void {
    this.initializeTable();
  }
  
  deleteOrder(id:number){
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
        // Kullanıcı onay verdiyse silme işlemini gerçekleştir
        this.orderService.delete(id).subscribe(() => {
          // Veriyi sildikten sonra tabloyu yeniden yükle
          this.initializeTable(); // Burada tabloyu yeniden başlatın
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
  
  orderCreateModal(){
    const dialogRef = this.dialog.open(OrderModalComponent, {
      width: '700px'
    });

    dialogRef.afterClosed().subscribe({
      next:(res)=>{
        this.initializeTable();
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
        this.initializeTable();
        Swal.fire({
          title: 'Hata!',
          text: 'İşlem tamamlanamadı.',
          icon: 'error',
          position: 'top-end',
          showConfirmButton: false,
          timer: 1500,
        });
      }
    })
  }
}
