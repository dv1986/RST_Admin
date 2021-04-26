import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { CategoryAttributeMappingAddComponent } from '../category-attribute-mapping-add/category-attribute-mapping-add.component';
import { AttributeService } from '../../attributes.service';
import { CategoryAttributeMappingEditComponent } from '../category-attribute-mapping-edit/category-attribute-mapping-edit.component';


@Component({
    selector: 'app-category-attribute-mapping-list',
    templateUrl: './category-attribute-mapping-list.component.html'
})
export class CategoryAttributeMappingListComponent implements OnInit {


    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];
    // filterComponent: GridFilterItem = new GridFilterItem(CountrySearchComponent, this);

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _attributeService: AttributeService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.fillComboAttributeParent();
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this._attributeService.getAllAttributeProductTypeMapping().subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    this.dataList = response.data;
                }
                else {
                    this.dataList = [];
                }
        });
    }

    onEdit() {
        if (this.gridInformation.selectedData.length == 0) {
            this.notificationService.ShowError("Please select record(s) to update data!", 3000);
            return;
        }
        console.log(this.gridInformation.selectedData[0]);
        var initialState = {
            isModal: true,
            isEditMode: false,
            categoryParentId: this.gridInformation.selectedData[0].productCategoryParentId,
            categoryId: this.gridInformation.selectedData[0].productCategoryId,
            subCategoryParentId: this.gridInformation.selectedData[0].productSubCategoryParentId,
            subCategoryId: this.gridInformation.selectedData[0].productSubCategoryId,
            productTypeId: this.gridInformation.selectedData[0].productTypeId
        };
        this.modalRef = this.modalService.show(CategoryAttributeMappingEditComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-md" })
        if (this.modalRef) {
            this.modalRef.content.onAdd.subscribe(result => {
                this.onRefresh();
            })
        }
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
        this.modalRef = this.modalService.show(CategoryAttributeMappingAddComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-md" })
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
                title: "Update Attributes",
                cssClass: "btn btn-success btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onEdit();
                },
                requiresConfirmation: false,
                isDisabled: false

            }
        ];
    }

    initGrid() {
        this.gridOptions.columnDefs = [
            { headerName: '', field: 'rowId', hide: true },
            { headerName: '', field: 'productTypeId', hide: true },
            { headerName: '', field: 'productSubCategoryId', hide: true },
            { headerName: '', field: 'productSubCategoryParentId', hide: true },
            { headerName: '', field: 'productCategoryId', hide: true },
            { headerName: '', field: 'productCategoryParentId', hide: true },

            { headerName: 'ProductAttributeId', field: 'productAttributeId', hide: true },
            { headerName: 'Category Parent', field: 'categoryParentName' },
            { headerName: 'Category', field: 'categoryName' },
            { headerName: 'Sub-Category Parent', field: 'subCategoryParentName' },
            { headerName: 'Sub-Category', field: 'subCategoryName' },
            { headerName: 'Product Type', field: 'productTypeName' },
            { headerName: 'Attribute Name', field: 'attributeName' },
        ];
        this.gridOptions.defaultColDef = { resizable: true };
    }
    //#endregion

    AttributeParentList: any[] = [];
    fillComboAttributeParent() {
        this._attributeService.getproductAttributeParent("").subscribe(response => {
            if (response.state == 0) {
                response.data.forEach(element => {
                    this.AttributeParentList.push(`${element.rowId}-${element.name}`);
                });
            }
        });
    }

    lookUpValueGetterforAttributeParent(params) {
        if (params.data !== undefined) {
            return params.data.productAttributeParentName;
        }
    }

    lookUpValueSetterforAttributeParent(params) {
        if (params.data !== undefined) {
            var id = params.newValue.split('-')[0].trim();
            params.data.productAttributeParentId = Number(id);

            var text = params.newValue.split('-')[1].trim();
            params.data.productAttributeParentName = text;
            return params.data.productAttributeParentName;
        }
    }
}
