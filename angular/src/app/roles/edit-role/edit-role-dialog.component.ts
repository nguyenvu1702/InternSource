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
  GetRoleForEditOutput,
  RoleDto,
  RoleEditDto,
  StringFlatTreeSelectDto
} from '@shared/service-proxies/service-proxies';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CommonComponent } from '@shared/dft/components/common.component';
import { PermissionTreeComponent } from '../lib/permission-tree.component';

@Component({
  templateUrl: 'edit-role-dialog.component.html'
})
export class EditRoleDialogComponent extends AppComponentBase
  implements OnInit {
  @ViewChild('permissionTree', { static: true }) permissionTree: PermissionTreeComponent;
  @Output() onSave = new EventEmitter<any>();
  saving = false;
  isPermissionActive = false;
  id: number;
  role = new RoleEditDto();
  permissionChecked: StringFlatTreeSelectDto[] = [];

  form: FormGroup;

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private _roleService: RoleServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.khoiTaoForm();
    this._roleService
      .getRoleForEdit(this.id)
      .subscribe((result: GetRoleForEditOutput) => {
        this.role = result.role;
        this.permissionTree.editData = { permissions: result.permissions, grantedPermissionNames: result.grantedPermissionNames };
        this._setValueForEdit();
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
      this._roleService.checkExist(this.role.name, this.role.displayName, this.role.id).subscribe((res) => {
        switch (res) {
          case 0: {
            const role = new RoleDto();
            role.init(this.role);
            role.grantedPermissions = this.getCheckedPermissions();

            this._roleService
              .update(role).subscribe(() => {
                this.showUpdateMessage();
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

  private _setValueForEdit() {
    this.form.controls.TenVaiTro.setValue(this.role.name);
    this.form.controls.TenHienThi.setValue(this.role.displayName);
    this.form.controls.GhiChu.setValue(this.role.description);
  }

  private _getValueForSave() {
    this.role.name = this.form.controls.TenVaiTro.value;
    this.role.displayName = this.form.controls.TenHienThi.value;
    this.role.description = this.form.controls.GhiChu.value;
  }
}
