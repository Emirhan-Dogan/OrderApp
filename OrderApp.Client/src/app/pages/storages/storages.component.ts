import { Component, inject, ViewChild, AfterViewInit, ChangeDetectionStrategy, ViewEncapsulation } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { StorageModalComponent } from 'src/app/features/storage/modals/storage-modal/storage-modal.component';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { StorageUpdateModalComponent } from 'src/app/features/storage/modals/storage-update-modal/storage-update-modal.component';
import { StorageService } from 'src/app/features/storage/service/storage.service';
import { IStorage } from 'src/app/core/models/IStorage';
import { AuthService } from 'src/app/core/services/auth.service';
declare var $: any;

@Component({
  selector: 'app-storages',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './storages.component.html',
  styleUrls: ['./storages.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class StoragesComponent implements AfterViewInit {
  dialog = inject(MatDialog);
  router = inject(Router);
  storages: IStorage[] = [];
  private dataTable: any;

  constructor(private storageService: StorageService, private authService:AuthService) {}

  ngAfterViewInit(): void {
    this.initializeTable();
  }

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }

  initializeTable() {
    this.storageService.getAll().subscribe((res) => {
      console.log(res);
      this.storages = res;
      if (this.dataTable) {
        this.dataTable.destroy(); // Mevcut tabloyu yok et
      }
      this.dataTable = $('#datatable-buttons').DataTable({
        data: this.storages,
        columns: [
          { title: 'İsim', data: 'name' },
          {
            title: 'Durum',
            data: 'status',
            render: function (data: boolean, _type: any, row: any) {
              return data
                ? '<span class="status-active">Aktif</span>'
                : '<span class="status-inactive">Pasif</span>';
            },
          },
          { title: 'Şehir', data: 'address.city.name' },
          { title: 'Ülke', data: 'address.country.name' },
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
            },
          },
        ],
        drawCallback: () => {
          // Detay butonu için olay bağlama
          $('.detail-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            this.detailStorage(id);
          });

          // Düzenle butonu için olay bağlama
          $('.update-btn').on('click', (event: any) => {
            const id = $(event.currentTarget).data('id');
            if(this.checkClaim('UpdateStorageCommand')){
              this.updateStorage(id);
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
            if(this.checkClaim('DeleteStorageCommand')){
              this.deleteStorage(id);
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
      });
    });
  }

  deleteStorage(id: number) {
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
        this.storageService.delete(id).subscribe(() => {
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

  updateStorage(id: number) {
    const storage = this.storages.find((s) => s.id === id);

    if (storage) {
      this.dialog
        .open(StorageUpdateModalComponent, {
          width: '500px',
          data: storage,
          disableClose: true
        })
        .afterClosed()
        .subscribe({
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
    } else {
      console.error('Storage not found');
    }
  }

  detailStorage(id: number) {
    this.router.navigate(['/storage-detail', id]);
  }

  storageCreateModal() {
    this.dialog.open(StorageModalComponent, {
      width: '500px',
      disableClose: true
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
}
