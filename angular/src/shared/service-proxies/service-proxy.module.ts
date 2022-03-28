import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module';

import * as ApiServiceProxies from './service-proxies';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ApiServiceProxies.DemoServiceProxy,
        ApiServiceProxies.LookupTableServiceProxy,
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
    ]
})
export class ServiceProxyModule { }
