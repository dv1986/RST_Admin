import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PushNotificationService } from '../notification.service';


@Component({
    selector: 'app-notification-list',
    templateUrl: './notification-list.component.html'
})
export class NotificationListComponent implements OnInit {

    searchCriteria = {
        countryName: ""
    };

    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];
    // filterComponent: GridFilterItem = new GridFilterItem(CountrySearchComponent, this);

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _notificationService: PushNotificationService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this._notificationService.getNotification(this.searchCriteria.countryName).subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    this.dataList = response.data;
                }
                else {
                    this.dataList = [];
                }
        });
    }

    onUpdate() {
        if (this.gridInformation.selectedData.length == 0) {
            this.notificationService.ShowError("Please select record(s) to update data!", 3000);
            return;
        }
        this._notificationService.updateNotification(this.gridInformation.updatedData).subscribe(response => {
            if (response.state == 0) {
                this.onRefresh();
                this.notificationService.ShowSuccess("Record updated sucessfully!", 3000);
            }
            else if (response.state == 4) {
                for (let rowindex in response.data) {
                    let item = this.dataList.find(fn => fn.rowId == response.data[rowindex].rowId)
                    item.message = response.data[rowindex].message;
                    if (item.message != "")
                        this.notificationService.ShowWarning("Please check Message Column", 3000)
                }
                this.gridOptions.api.refreshCells({ force: true });
            }
        });
    }

    onDelete() {
        if (this.gridInformation.selectedData.length == 0) {
            this.notificationService.ShowError("Please select record(s) to delete data!", 3000);
            return;
        }
        this._notificationService.deleteNotification(this.gridInformation.selectedData).subscribe(response => {
            if (response.state == 0) {
                this.notificationService.ShowSuccess("Record deleted sucessfully!", 3000);
                this.onRefresh();
            }
            else if (response.state == 4) {
                for (let rowindex in response.data) {
                    let item = this.dataList.find(fn => fn.rowId == response.data[rowindex].rowId)
                    item.message = response.data[rowindex].message;
                    if (item.message != "")
                        this.notificationService.ShowWarning("Please check Message Column", 3000)
                }
                this.gridOptions.api.refreshCells({ force: true });
            }
        });
    }
    onRefresh() {
        this.loadData();
        this.gridInformation.selectedData = [];
        this.gridInformation.updatedData = [];
        this.gridOptions.api.deselectAll();
    }

    //#region Required function to initialize grid and actions
    onGridReady() {
        this.gridOptions.context["mainComponent"] = this;
    }

    initGridActions() {
        this.gridActions = [
            {
                actionId: 1,
                title: "Refresh",
                cssClass: "btn btn-warning btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onRefresh();
                },
                requiresConfirmation: false,
                isDisabled: false
            },
            {
                actionId: 2,
                title: "Update",
                cssClass: "btn btn-success btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onUpdate();
                },
                requiresConfirmation: false,
                isDisabled: false

            },
            {
                actionId: 3,
                title: "Delete",
                cssClass: "btn btn-danger btn-xs",
                onClick: function (event, context) {

                },
                requiresConfirmation: true,
                confirmationPlacement: "bottom",/*bottom,left,top,right*/
                confirmationTitle: "Confirm",
                confirmationMessage: "Do you want to continue delete the selected record?",
                confirmationYesAction: function (event, context) {
                    context.mainComponent.onDelete();
                },
                isDisabled: false
            }
        ];
    }

    initGrid() {
        this.gridOptions.columnDefs = [
            { headerName: 'RowId', field: 'rowId', hide:true },
            { headerName: 'ProductId', field: 'productId', hide:true },
            { headerName: 'ProductTitle', field: 'productTitle' },
            { headerName: 'SKUCode', field: 'skuCode' },
            // { headerName: 'ShortDescription', field: 'shortDescription' },
            { headerName: 'ModelNumber', field: 'modelNumber' },
            { headerName: 'ModelYear', field: 'modelYear' },
            { headerName: 'TextPrompt', field: 'textPrompt' },
            {
                headerName: 'Is Active', field: 'isActive', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsActive
            },
            { headerName: 'message', field: 'message', type: ["Error"] }
        ];
        this.gridOptions.defaultColDef = { resizable: true };
    }
    //#endregion

    fillcombostatus(param) {
        return {
            values: [true, false]
        }
    }

    lookUpValueGetterforIsActive(params) {
        if (params.data !== undefined)
            return params.data.isActive;
    }
}
