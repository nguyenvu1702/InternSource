/* tslint:disable */
import { StringFlatTreeSelectDto } from '@shared/service-proxies/service-proxies';
export interface PermissionTreeEditModel {
    permissions: StringFlatTreeSelectDto[];
    grantedPermissionNames: string[];
}
