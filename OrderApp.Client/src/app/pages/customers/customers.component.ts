import { AfterViewInit, Component, inject, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ICustomer } from 'src/app/core/models/ICustomer';
import { AuthService } from 'src/app/core/services/auth.service';
import { CustomerModalComponent } from 'src/app/features/customer/modals/customer-modal/customer-modal.component';
import { CustomerUpdateModalComponent } from 'src/app/features/customer/modals/customer-update-modal/customer-update-modal.component';
import { CustomerService } from 'src/app/features/customer/service/customer.service';
import Swal from 'sweetalert2';
declare var $:any;
@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
  encapsulation: ViewEncapsulation.None
})
export class CustomersComponent implements AfterViewInit{
  dataTable:any;
  dialog = inject(MatDialog);
  router = inject(Router);
  public customers: ICustomer[] = [];

  constructor(
    private customerService: CustomerService,
    private authService:AuthService
  ) {
    
  }
  ngAfterViewInit(): void {
    this.initializeTable();
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  initializeTable() {
    this.customerService.getAll().subscribe((res) => {
      this.customers = res; // Verileri alın
  
      // Eğer tablo daha önce oluşturulmuşsa önce yok edin
      if (this.dataTable) {
        this.dataTable.destroy();
      }
  
      // DataTable'ı başlatın
      this.dataTable = $('#datatable-buttons').DataTable({
        data: this.customers, // Müşteri verilerini tabloya yükleyin
        columns: [
          { title: 'İsim', data: 'user.fullName' },
          { title: 'Email', data: 'user.email' },
          { title: 'Müşteri Kodu', data: 'customerCode' },
          { 
            title: 'Cinsiyet', 
            data: 'user.gender',
            render: function (data: number) {
              // Gender verisini sayısal bir değerden insan tarafından okunabilir hale getirin
              return data === 1 ? 'Erkek' : data === 2 ? 'Kadın' : 'Diğer';
            }
          },
          { 
            title: 'Oluşturma Zamanı', 
            data: 'createdDate',
            render: function (data: string) {
              const formattedDate = new Date(data).toLocaleDateString('tr-TR');
              return formattedDate; // Tarihi dd/MM/yyyy formatında göster
            }
          },
          { 
            title: 'Durum', 
            data: 'status',
            render: function (data: boolean) {
              return data
                ? '<span class="status-active">Aktif</span>'
                : '<span class="status-inactive">Pasif</span>';
            }
          },
          {
            title: 'Daha Fazla',
            data: 'id',
            render: function (data: number) {
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
            this.customerDetails(id);
          });
  
          // Düzenle butonu için olay bağlama
          $('.update-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('UpdateCustomerCommand')){
              this.updateCustomer(id);
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
  
          // Sil butonu için olay bağlama
          $('.delete-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('DeleteCustomerCommand')){
              this.deleteCustomer(id);
            }else{
              Swal.fire({
                title: 'Yetki Gerekiyor!',
                text: 'Bu işlemi gerçekleştirmek için izniniz yok.',
                icon: 'warning',
                confirmButtonText: 'Tamam'
              });              
            }
          });
        }
      });
    });
  }
  customerDetails(id:number){
    this.router.navigate(['/customer-detail', id])
  }

  deleteCustomer(id:number){
    Swal.fire({
      title: 'Emin misiniz?',
      text: "Bu işlem geri alınamaz!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText: 'İptal'
    }).then((result) => {
      if (result.isConfirmed) {
        // Kullanıcı onay verdiyse silme işlemini gerçekleştir
        this.customerService.delete(id).subscribe({
          next:()=>{
            this.initializeTable();
            Swal.fire({
              title: 'Başarılı!',
              text: 'İşlem başarılı bir şekilde tamamlandı.',
              icon: 'success',
              position: 'top-end',
              showConfirmButton: false,
              timer: 1500,
            });
          },
          error:()=>{
            this.initializeTable();
            Swal.fire({
              title: 'Hata!',
              text: 'Bir hata oluştu',
              icon: 'error',
              position: 'top-end',
              showConfirmButton: false,
              timer: 1500,
            });
          }
        });
      }
    });
  }

  updateCustomer(id:number){
    const customer = this.customers.find(O=>O.id===id);
    this.dialog.open(CustomerUpdateModalComponent,{
      width:'750px',
      data:customer
    }).afterClosed().subscribe({
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
  customerCreateModal(){
    this.dialog.open(CustomerModalComponent, {
      width: '700px'
    }).afterClosed().subscribe({
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
    });
  }
}
