// tslint:disable
import { LookupTableDto } from './../../shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { OnInit, ViewChild } from '@angular/core';
import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  UserServiceProxy,
  UserDto,
  UserDtoPagedResultDto,
  LookupTableServiceProxy
} from '@shared/service-proxies/service-proxies';
import { CreateUserDialogComponent } from './create-user/create-user-dialog.component';
import { EditUserDialogComponent } from './edit-user/edit-user-dialog.component';
import { ResetPasswordDialogComponent } from './reset-password/reset-password.component';
import { AppComponentBase } from '@shared/app-component-base';
import { Table } from 'primeng/table';
@Component({
  templateUrl: './users.component.html',
  animations: [appModuleAnimation()]
})
export class UsersComponent extends AppComponentBase implements OnInit {
  @ViewChild('dt') table: Table;
  users: UserDto[] = [];
  keyword = '';
  isActive: boolean | null;
  advancedFiltersVisible = false;
  loading = true;
  totalCount = 0;
  constructor(
    injector: Injector,
    private _userService: UserServiceProxy,
    private _lookupTableServiceProxy: LookupTableServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }
  ngOnInit(): void {
  }

  getDataPage(lazyLoad?: LazyLoadEvent) {
    this.first = 0;
    this.loading = true;
    this._userService
      .getAll(
        this.keyword || undefined,
        this.isActive || undefined,
        this.getSortField(this.table),
        lazyLoad ? lazyLoad.first : this.first,
        lazyLoad ? lazyLoad.rows : this.table.rows,
      ).pipe(finalize(() => {
        this.loading = false;
      })).subscribe((result: UserDtoPagedResultDto) => {
        this.users = result.items;
        this.totalCount = result.totalCount;
      });
  }

  createUser(): void {
    this.showCreateOrEditUserDialog();
  }

  editUser(user: UserDto): void {
    this.showCreateOrEditUserDialog(user.id);
  }

  viewUser(user: UserDto): void {
    this.showCreateOrEditUserDialog(user.id, true);
  }

  updateRole(user: UserDto) {
    this.showCreateOrEditUserDialog(user.id, false, true);
  }

  public resetPassword(user: UserDto): void {
    this.showResetPasswordUserDialog(user.id);
  }

  protected delete(user: UserDto): void {
    if (user.lastLoginTime) {
      this.showSwalAlertMessage('Người dùng đã đăng nhập, không được xóa!', 'error');
    } else {
      this.swal.fire({
        title: 'Bạn chắc chắn không?',
        text: 'Người dùng ' + user.name + ' sẽ bị xóa.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: this.confirmButtonColor,
        cancelButtonColor: this.cancelButtonColor,
        cancelButtonText: this.cancelButtonText,
        confirmButtonText: this.confirmButtonText
      }).then((result) => {
        if (result.value) {
          this._userService.delete(user.id).subscribe(() => {
            this.showDeleteMessage();
            this.getDataPage();
          });
        }
      });
    }
  }

  private showResetPasswordUserDialog(id?: number): void {
    this._modalService.show(ResetPasswordDialogComponent, {
      class: 'modal-xl',
      initialState: {
        id: id,
      },
    });
  }

  private showCreateOrEditUserDialog(id?: number, isView = false, isRoleActive = false): void {
    let createOrEditUserDialog: BsModalRef;
    if (!id) {
      createOrEditUserDialog = this._modalService.show(
        CreateUserDialogComponent,
        {
          class: 'modal-xl',
        }
      );
    } else {
      createOrEditUserDialog = this._modalService.show(
        EditUserDialogComponent,
        {
          class: 'modal-xl',
          initialState: {
            id: id,
            isView,
            isRoleActive
          },
        }
      );
    }

    createOrEditUserDialog.content.onSave.subscribe(() => {
      this.getDataPage();
    });
  }
}
