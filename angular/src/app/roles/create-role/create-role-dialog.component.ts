// tslint:disable
import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ViewChild,
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import * as _ from 'lodash';
import { AppComponentBase } from '@shared/app-component-base';
import {
  RoleServiceProxy,
  RoleDto,
  CreateRoleDto,
  StringFlatTreeSelectDto,
} from '@shared/service-proxies/service-proxies';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonComponent } from '@shared/dft/components/common.component';
import { PermissionTreeComponent } from '@app/roles/lib/permission-tree.component';

@Component({
  templateUrl: 'create-role-dialog.component.html'
})
export class CreateRoleDialogComponent extends AppComponentBase
  implements OnInit {
  @ViewChild('permissionTree', { static: true }) permissionTree: PermissionTreeComponent;
  saving = false;
  role = new RoleDto();
  @Output() onSave = new EventEmitter<any>();
  form: FormGroup;
  permissionChecked: StringFlatTreeSelectDto[] = [];

  constructor(
    injector: Injector,
    private _roleService: RoleServiceProxy,
    private fb: FormBuilder,
    public bsModalRef: BsModalRef,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.khoiTaoForm();
    this._roleService
      .getAllPermissions()
      .subscribe((result: StringFlatTreeSelectDto[]) => {
        this.permissionTree.editData = { permissions: result, grantedPermissionNames: [] };
      });
  }

  khoiTaoForm() {
    this.form = this.fb.group({
      TenVaiTro: ['', Validators.required],
      TenHienThi: ['', Validators.required],
      GhiChu: [''],
    });
  }

  save(): void {
    if (CommonComponent.getControlErr(this.form) === '') {
      this.saving = true;
      this._getValueForSave();
      this._roleService.checkExist(this.role.name, this.role.displayName, 0).subscribe((res) => {
        switch (res) {
          case 0: {
            const role = new CreateRoleDto();
            role.init(this.role);
            role.grantedPermissions = this.getCheckedPermissions();
            this._roleService
              .create(role).subscribe(() => {
                this.showCreateMessage();
                this.bsModalRef.hide();
                this.onSave.emit();
              });
            break;
          }
          case 1: {
            this.showSwalAlertMessage('Tên vai trò đã tồn tại!', 'error');
            this.saving = false;
            break;
          }
          case 2: {
            this.showSwalAlertMessage('Tên hiển thị đã tồn tại!', 'error');
            this.saving = false;
            break;
          }
          default:
            break;
        }
      });
    }
  }

  private getCheckedPermissions(): string[] {
    return this.permissionChecked.map(e => e.id);
  }

  private _getValueForSave() {
    this.role.name = this.form.controls.TenVaiTro.value;
    this.role.displayName = this.form.controls.TenHienThi.value;
    this.role.description = this.form.controls.GhiChu.value;
  }
}
