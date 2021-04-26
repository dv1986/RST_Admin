import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';
import { getNumericCellEditor } from 'src/app/shared/components/grid/components';
import { ImageViewer } from 'src/app/shared/components/ImageViewer/ImageViewer';
import { Imagecompressionpopup } from 'src/app/shared/components/image-compression-popup/image-compression-popup.component';
import { ParentSubCategoryAddComponent } from '../parent-subcategory-add/parent-subcategory-add.component';


@Component({
    selector: 'app-parent-subcategory-list',
    templateUrl: './parent-subcategory-list.component.html'
})
export class ParentSubCategoryListComponent implements OnInit {

    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _productService: ProductService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.fillcomboCategory();
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this._productService.getProductSubCategoryParent("").subscribe(response => {
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
        if (this.gridInformation.updatedData.length == 0) {
            this.notificationService.ShowError("Please modify record(s) to update data!", 3000);
            return;
        }
        this._productService.updateProductSubCategoryParent(this.gridInformation.updatedData).subscribe(response => {
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
        this._productService.deleteProductSubCategoryParent(this.gridInformation.selectedData).subscribe(response => {
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
        this.modalRef = this.modalService.show(ParentSubCategoryAddComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" })
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

    private modalRef1: BsModalRef;
    onImageClick(params) {
        debugger;
        const initialState = {
            ImagePath: params.data.imagePath,
            ImageName: params.data.imageName,
            moduleName: "ParentSubCategory",
            rowId: params.data.rowId
        };
        params.context["mainComponent"].ShowImageModalRef = params.context["mainComponent"].modalService.show(Imagecompressionpopup, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" })
        if (params.context["mainComponent"].ShowImageModalRef) {
            params.context["mainComponent"].ShowImageModalRef.content.onAdd.subscribe(result => {
                params.context["mainComponent"].onRefresh();
            })
        }
    }

    initGrid() {
        this.gridOptions.columnDefs = [
            { headerName: '', field: 'rowId', hide: true },
            { headerName: 'Image', field: 'thumbnailPath', onCellClicked: this.onImageClick, cellRendererFramework: ImageViewer },
            { headerName: 'ProductCategoryId', field: 'productCategoryId', hide: true },
            { headerName: 'ProductCategoryParentId', field: 'productCategoryParentId', hide: true },
            { headerName: 'Cat Parent Name', field: 'catParentName' },
            {
                headerName: 'Category Name', field: 'catName', sortable: true, filter: true, type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: { values: this.CategoryList }, valueGetter: this.lookUpValueGetterforCategory,
                valueSetter: this.lookUpValueSetterforCategory
            },
            { headerName: 'Sub-Category Name', field: 'subCategoryParentName', sortable: true, filter: true, type: ['editableColumn'] },
            { headerName: 'Meta Title', field: 'metaTitle', sortable: true, filter: true },
            { headerName: 'Meta Keyword', field: 'metaKeyword', sortable: true, filter: true },
            { headerName: 'Description', field: 'description', resizable: false, type: ['editableColumn'] },
            {
                headerName: 'includeInTopMenu', field: 'includeInTopMenu', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIncludeInTopMenu
            },
            {
                headerName: 'IsNew', field: 'isNew', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsNew
            },
            {
                headerName: 'HasDiscountApplied', field: 'hasDiscountApplied', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforHasDiscountApplied
            },
            {
                headerName: 'IsPublished', field: 'isPublished', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsPublished
            },
            { headerName: 'DisplayOrder', field: 'displayOrder', type: ['editableColumn'], cellEditor: "numericCellEditor" },
            { headerName: 'CreatedOnUtc', field: 'createdOnUtc', type: ["Date"] },
            { headerName: 'ModifiedOnUtc', field: 'modifiedOnUtc', type: ["Date"] },
            { headerName: 'message', field: 'message', type: ["Error"] }
        ];
        this.gridOptions.defaultColDef = { resizable: true };
        this.gridOptions.components = {
            numericCellEditor: getNumericCellEditor()
        };
    }
    //#endregion

    lookUpValueGetterforIncludeInTopMenu(params) {
        if (params.data !== undefined)
            return params.data.includeInTopMenu;
    }
    lookUpValueGetterforIsNew(params) {
        if (params.data !== undefined)
            return params.data.isNew;
    }
    lookUpValueGetterforHasDiscountApplied(params) {
        if (params.data !== undefined)
            return params.data.hasDiscountApplied;
    }
    lookUpValueGetterforIsPublished(params) {
        if (params.data !== undefined)
            return params.data.isPublished;
    }



    fillcombostatus(param) {
        return {
            values: [true, false]
        }
    }

    CategoryList: any[] = [];
    fillcomboCategory() {
        this._productService.getProductCategory("").subscribe(response => {
            if (response.state == 0) {
                response.data.forEach(element => {
                    this.CategoryList.push(`${element.rowId}-${element.catName}`);
                });
            }
        });
    }

    lookUpValueGetterforCategory(params) {
        if (params.data !== undefined) {
            return params.data.catName;
        }
    }

    lookUpValueSetterforCategory(params) {
        if (params.data !== undefined) {
            var id = params.newValue.split('-')[0].trim();
            params.data.productCategoryId = Number(id);

            var text = params.newValue.split('-')[1].trim();
            params.data.catName = text;
            return params.data.catName;
        }
    }
}
