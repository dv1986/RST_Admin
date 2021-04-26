import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { getNumericCellEditor } from 'src/app/shared/components/grid/components';
import { SpecificationService } from 'src/app/specification/specification.service';
import { SizeTypeAddComponent } from '../size-type-add/size-type-add.component';


@Component({
    selector: 'app-size-type-list',
    templateUrl: './size-type-list.component.html'
})
export class SizeTypeListComponent implements OnInit {


    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _specificationService: SpecificationService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.initGrid();
        this.fillcomboParentCategory();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this._specificationService.getProductSizeType("").subscribe(response => {
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
        this._specificationService.updateProductSizeType(this.gridInformation.updatedData).subscribe(response => {
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
        this._specificationService.deleteProductSizeType(this.gridInformation.selectedData).subscribe(response => {
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

    private modalRef: BsModalRef;
    onAddNew() {
        //this.onAdd.emit();
        var initialState = {
            isModal: true,
            isEditMode: false
        };
        this.modalRef = this.modalService.show(SizeTypeAddComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-md" })
        if (this.modalRef) {
            this.modalRef.content.onAdd.subscribe(result => {
                this.onRefresh();
            })
        }
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
            { headerName: '', field: 'measureDimensionId', hide: true },
            { headerName: 'Type Name', field: 'typeName', sortable: true, filter: true, type: ['editableColumn'] },
            { headerName: 'Type Code', field: 'typeCode', sortable: true, filter: true, type: ['editableColumn'] },
            {
                headerName: 'Measurement Dimension', field: 'measurementDimension', sortable: true, filter: true, type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: { values: this.measurementDimensionList }, valueGetter: this.lookUpValueGetterforMeasurementDimension,
                valueSetter: this.lookUpValueSetterforMeasurementDimension
            },
            { headerName: 'Description', field: 'description', type: ['editableColumn'] },
            { headerName: 'message', field: 'message', type: ["Error"] }
        ];
        this.gridOptions.defaultColDef = { resizable: true };
    }
    //#endregion

    measurementDimensionList: any[] = [];
    fillcomboParentCategory() {
        this._specificationService.getMeasureDimension("").subscribe(response => {
            if (response.state == 0) {
                response.data.forEach(element => {
                    this.measurementDimensionList.push(`${element.rowId}-${element.name}`);
                });
            }
        });
    }

    lookUpValueGetterforMeasurementDimension(params) {
        if (params.data !== undefined) {
            return params.data.measurementDimension;
        }
    }

    lookUpValueSetterforMeasurementDimension(params) {
        if (params.data !== undefined) {
            var id = params.newValue.split('-')[0].trim();
            params.data.measureDimensionId = Number(id);

            var text = params.newValue.split('-')[1].trim();
            params.data.measurementDimension = text;
            return params.data.measurementDimension;
        }
    }
}
