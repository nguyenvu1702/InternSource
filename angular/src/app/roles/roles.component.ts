// tslint:disable
import { AppComponentBase } from '@shared/app-component-base';
import { finalize } from 'rxjs/operators';
import { Component, Injector, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  RoleServiceProxy,
  RoleDto,
  RoleDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateRoleDialogComponent } from './create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './edit-role/edit-role-dialog.component';
import { LazyLoadEvent } from 'primeng/api/public_api';
import { Table } from 'primeng/table';
@Component({
  templateUrl: './roles.component.html',
  animations: [appModuleAnimation()]
})
export class RolesComponent extends AppComponentBase {
  @ViewChild('dt') table: Table;
  roles: RoleDto[] = [];
  keyword = '';
  loading = true;
  totalCount = 0;
  constructor(
    injector: Injector,
    private _rolesService: RoleServiceProxy,
    private _modalService: BsModalService
  ) {
    super(injector);
  }

  getDataPage(lazyLoad?: LazyLoadEvent) {
    this.loading = true;
    this._rolesService
      .getAll(
        this.keyword || undefined,
        this.getSortField(this.table),
        lazyLoad ? lazyLoad.first : this.table.first,
        lazyLoad ? lazyLoad.rows : this.table.rows,
      ).pipe(finalize(() => {
        this.loading = false;
      })).subscribe((result: RoleDtoPagedResultDto) => {
        this.roles = result.items;
        this.totalCount = result.totalCount;
      });
  }

  delete(role: RoleDto): void {
    this.swal.fire({
      title: 'Bạn chắc chắn không?',
      text: 'Vai trò ' + role.displayName + ' sẽ bị xóa.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: this.confirmButtonColor,
      cancelButtonColor: this.cancelButtonColor,
      cancelButtonText: this.cancelButtonText,
      confirmButtonText: this.confirmButtonText
    }).then((result) => {
      if (result.value) {
        this._rolesService
          .delete(role.id).subscribe(() => {
            this.showDeleteMessage();
            this.getDataPage();
          });
      }
    });

  }

  createRole(): void {
    this.showCreateOrEditRoleDialog();
  }

  editRole(role: RoleDto, isPermissionActive = false): void {
    this.showCreateOrEditRoleDialog(role.id, isPermissionActive);
  }

  showCreateOrEditRoleDialog(id?: number, isPermissionActive = false): void {
    let createOrEditRoleDialog: BsModalRef;
    if (!id) {
      createOrEditRoleDialog = this._modalService.show(
        CreateRoleDialogComponent,
        {
          class: 'modal-xl',
        }
      );
    } else {
      createOrEditRoleDialog = this._modalService.show(
        EditRoleDialogComponent,
        {
          class: 'modal-xl',
          initialState: {
            id: id,
            isPermissionActive
          },
        }
      );
    }

    createOrEditRoleDialog.content.onSave.subscribe(() => {
      this.getDataPage();
    });
  }
}
