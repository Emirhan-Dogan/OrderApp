import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatOptionModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { ActivatedRoute } from '@angular/router';
import { IUser } from 'src/app/core/models/IUser';
import { AuthService } from 'src/app/core/services/auth.service';
import { GroupService } from 'src/app/core/services/group.service';
import { UserGroupService } from 'src/app/core/services/user-group.service';
import { UserService } from 'src/app/features/user/service/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-claim-detail',
  standalone: true,
  imports: [CommonModule, MatFormFieldModule, MatSelectModule, FormsModule],
  templateUrl: './user-claim-detail.component.html',
  styleUrls: ['./user-claim-detail.component.scss'] // Düzeltildi
})
export class UserClaimDetailComponent implements OnInit {
  userId: number = 0;
  user?: IUser;
  roles: any[] = []; // Grupları çek
  groupId:any;
  constructor(private userService: UserService, private route: ActivatedRoute,
    private authService: AuthService, private groupService: GroupService,
    private userGroupService: UserGroupService) {
  }

  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim)
  }

  ngOnInit(): void {
    this.userId = parseInt(this.route.snapshot.paramMap.get('id') as string);
    this.userService.getById(this.userId).subscribe(
      res => {
        this.user = res;
      }
    )
    this.groupService.getAll().subscribe({
      next: (res) => {
        this.roles = res;
      }
    })
  }

  addUserClaim(form: any, id: number) {
    let userGroup = {
      userId: id,
      groupId: this.groupId
    }
    console.log(id);
    console.log(this.groupId);

    Swal.fire({
      title: 'Emin misiniz?',
      text: "Bu işlem geri alınamaz!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, Yetkilendir!',
      cancelButtonText: 'İptal'
    }).then((result) => {
      if (result.isConfirmed) {
        this.userGroupService.add(userGroup).subscribe(() => {
          Swal.fire({
            title: 'Başarılı!',
            text: 'Yetkilendirildi.',
            icon: 'success',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        }, (error) => {
          console.error('Yetkilendirme işlemi sırasında hata oluştu:', error);
          Swal.fire({
            title: 'Hata!',
            text: 'Yetkilendirme işlemi başarısız oldu.',
            icon: 'error',
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500,
          });
        });
      }
    });
  }
}
