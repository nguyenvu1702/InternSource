/* tslint:disable */
import { CommonComponent } from './../../../shared/dft/components/common.component';
import { StringLookupTableDto } from './../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, OnInit } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/app-component-base";
import { FileDownloadService } from "@shared/file-download.service";
import { AuditLogServiceProxy, AuditLogListDto, GetAuditLogsInput, } from "@shared/service-proxies/service-proxies";
import { LazyLoadEvent } from "primeng/api";
import { Table } from "primeng/table";
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './audit-logs.component.html',
    styleUrls: ['./audit-logs.component.less'],
    animations: [appModuleAnimation()]
})
export class AuditLogsComponent extends AppComponentBase implements OnInit {

    @ViewChild('dt') table: Table;
    public usernameAuditLog: string;
    public serviceName: StringLookupTableDto;
    totalCount = 0;
    primengTableHelperAuditLogs: AuditLogListDto[] = [];
    advancedFiltersAreShown = false;
    loading = true;
    exporting = false;
    keyword = '';
    input: GetAuditLogsInput;
    arrService: StringLookupTableDto[] = [];
    rangeDates: any[] = [];
    first = 0;

    constructor(
        injector: Injector,
        private _auditLogService: AuditLogServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.rangeDates[0] = CommonComponent.getNgayDauTienCuaThangHienTaiDate();
        this.rangeDates[1] = new Date();

        this._auditLogService.getAllServiceName().subscribe(res => {
            this.arrService = res;
        })
    }

    getAuditLogs(lazyLoad?: LazyLoadEvent) {
        this.loading = true;
        this.first = 0;
        this._auditLogService.getAllAuditLogs(
            this.rangeDates ? this.rangeDates[0] : undefined,
            this.rangeDates ? this.rangeDates[1] : undefined,
            this.usernameAuditLog || undefined,
            this.serviceName ? this.serviceName.id : undefined,
            this.getSortField(this.table),
            lazyLoad ? lazyLoad.first : this.first,
            lazyLoad ? lazyLoad.rows : this.table.rows,
        ).pipe(finalize(() => {
            this.loading = false;
        })).subscribe((result) => {
            this.totalCount = result.totalCount;
            this.primengTableHelperAuditLogs = result.items;
        });
    }

    exportToExcelAuditLogs(): void {
        const self = this;
        self.exporting = true;
        this.input = new GetAuditLogsInput();
        this.input.startDate = this.rangeDates ? this.rangeDates[0] : undefined;
        this.input.endDate = this.rangeDates ? this.rangeDates[1] : undefined;
        this.input.userName = this.usernameAuditLog || undefined;
        this.input.serviceName = this.serviceName ? this.serviceName.id : undefined;
        this.input.sorting = this.getSortField(this.table);
        this.input.skipCount = 0;
        this.input.maxResultCount = 10000000;
        this.input.startDate = this.rangeDates ? this.rangeDates[0] : undefined;
        this.input.endDate = this.rangeDates ? this.rangeDates[1] : undefined;
        self._auditLogService.exportToExcel(this.input).pipe(finalize(() => {
            self.exporting = false;
        })).subscribe(result => {
            self._fileDownloadService.downloadTempFile(result);
        });
    }
}
