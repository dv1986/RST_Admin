import { Component, OnInit, Output, EventEmitter} from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { GridFilterItem } from 'src/app/shared/components/grid/grid-filters/GridFilterItem';
import { MasterService } from '../../master.service';
import { StateSearchComponent } from '../state-search/state-search.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StateAddComponent } from '../state-add/state-add.component';


@Component({
    selector: 'app-state-list',
    templateUrl: './state-list.component.html'
})
export class StateListComponent implements OnInit {

    searchCriteria = {
        countryName: ""
    };

    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];
    // filterComponent: GridFilterItem = new GridFilterItem(StateSearchComponent, this);

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _masterService: MasterService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.resetLocalStorage();
        this.loadData();
    }

    loadData() {
        this._masterService.getState(this.searchCriteria.countryName).subscribe(response => {
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
        this._masterService.updateState(this.gridInformation.updatedData).subscribe(response => {
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
        this._masterService.deleteState(this.gridInformation.selectedData).subscribe(response => {
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
        this.loadSearchCritariaFromLocalStorage();
        this.loadData();
        this.gridInformation.selectedData = [];
        this.gridInformation.updatedData = [];
        this.gridOptions.api.deselectAll();
    }

    private modalRef: BsModalRef;
    onAddNew(){
        //this.onAdd.emit();
        var initialState = {
            isModal: true,
            isEditMode: false
        };
        this.modalRef = this.modalService.show(StateAddComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-md" })
        if (this.modalRef) {
            this.modalRef.content.onAdd.subscribe(result => {
                this.onRefresh();
            })
        }
    }

    search() {
        if (this.gridOptions.context.thisComponent.showFilter == true)
            this.gridOptions.context.thisComponent.toggleFilter();
        this.onRefresh();
    }

    reset() {
        if (this.gridOptions.context.thisComponent.showFilter == true)
            this.gridOptions.context.thisComponent.toggleFilter();

        this.resetLocalStorage();
        this.onRefresh();
    }

    resetLocalStorage() {
        localStorage.removeItem("stateSearchQuery");
        this.searchCriteria.countryName = "";
    }

    loadSearchCritariaFromLocalStorage() {
        if (localStorage.getItem('stateSearchQuery')) {
            let searchRequest = JSON.parse(localStorage.getItem("stateSearchQuery"));
            this.searchCriteria.countryName = searchRequest.countryName;
        }
        console.log(this.searchCriteria);
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
                title: "Add New",
                cssClass: "btn btn-primary btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onAddNew();
                },
                requiresConfirmation: false,
                isDisabled: false
            },
            {
                actionId: 3,
                title: "Update",
                cssClass: "btn btn-success btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onUpdate();
                },
                requiresConfirmation: false,
                isDisabled: false

            },
            {
                actionId: 4,
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
            { headerName: '', field: 'rowId', hide: true },
            { headerName: 'Country Name', field: 'countryName', sortable: true, filter: true },
            { headerName: 'State Name', field: 'stateName', sortable: true, filter: true, type: ['editableColumn'] },
            { headerName: 'message', field: 'message', type: ["Error"] }
        ];
        this.gridOptions.defaultColDef = { resizable: true };
    }
    //#endregion
}
