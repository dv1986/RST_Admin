import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { getNumericCellEditor } from 'src/app/shared/components/grid/components';
import { ProductAttribteMappingListComponent } from '../../product-attribute-mapping-list/product-attribute-mapping-list.component';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { ProductImageMappingListComponent } from '../../product-images-mapping/product-images-mapping-list/product-images-mapping-list.component';


@Component({
    selector: 'app-product-list',
    templateUrl: './product-list.component.html'
})
export class ProductListComponent implements OnInit {
    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];

    @Output() onAdd = new EventEmitter<any>();
    @ViewChild(ProductAttribteMappingListComponent) productAttribteMappingListComponent: ProductAttribteMappingListComponent;
    @ViewChild(ProductImageMappingListComponent) productImageMappingListComponent: ProductImageMappingListComponent;

    constructor(private _productService: ProductService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        //this.fillcomboFeatureCategory();
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit(): void {
        this.loadData();
    }

    loadData() {
        this._productService.getProduct("").subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    this.dataList = response.data;
                }
                else {
                    this.dataList = [];
                }
        });
    }

    onSelectionChanged(params) {
        this.manageTabs('A');
    }

    @ViewChild('bottomtabset') tabset: TabsetComponent;
    public tabsList = {
        tabAttributes: true,
        tabColor: false,
        tabImages: false
    };

    manageTabs(params) {
        this.resetTabs();
        switch (params) {
            case "A":
                this.tabset.tabs[0].active = true;
                this.tabsList.tabAttributes = true;
                setTimeout(() => {
                    this.loadAttributesData();
                }, 300);
                break;
            case "C":
                this.tabset.tabs[1].active = true;
                this.tabsList.tabColor = true;

                break;
            case "I":
                this.tabset.tabs[2].active = true;
                this.tabsList.tabImages = true;
                setTimeout(() => {
                    this.loadImageData();
                }, 300);
                break;
        }
    }

    resetTabs() {
        this.tabsList.tabAttributes = false;
        this.tabsList.tabColor = false;
        this.tabsList.tabImages = false;
    }

    loadAttributesData() {
        if (this.productAttribteMappingListComponent != undefined)
            this.productAttribteMappingListComponent.loadData(this.gridInformation.selectedData[0].rowId);
    }

    loadImageData() {
        if (this.productImageMappingListComponent != undefined)
            this.productImageMappingListComponent.loadData(this.gridInformation.selectedData[0].rowId);
    }

    onUpdate() {
        if (this.gridInformation.updatedData.length == 0) {
            this.notificationService.ShowError("Please select record(s) to update data!", 3000);
            return;
        }
        this._productService.updateProduct(this.gridInformation.updatedData).subscribe(response => {
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
        this._productService.deleteProduct(this.gridInformation.selectedData).subscribe(response => {
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
        this.onAdd.emit();
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
                title: "Update Categories",
                cssClass: "btn btn-info btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onUpdateImages();
                },
                requiresConfirmation: false,
                isDisabled: false
            },
            {
                actionId: 5,
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
            { headerName: 'Category Parent Id', field: 'categoryParentId', hide: true },
            { headerName: 'Category Id', field: 'categoryId', hide: true },
            { headerName: 'SubCategory ParentId', field: 'subCategoryParentId', hide: true },
            { headerName: 'Sub Category Id', field: 'subCategoryId', hide: true },
            { headerName: 'Product Type Id', field: 'productTypeId', hide: true },
            { headerName: 'Brand Id', field: 'brandId', hide: true },
            { headerName: 'Product Tag Id', field: 'productTagId', hide: true },
            { headerName: 'SellerId', field: 'sellerId', hide: true },
            { headerName: 'DealProductCategoryId', field: 'dealProductCategoryId', hide: true },
            { headerName: 'CommisionRateId', field: 'commisionRateId', hide: true },
            { headerName: 'ProductFabricId', field: 'productFabricId', hide: true },
            { headerName: 'SeoContentId', field: 'seoContentId', hide: true },
            { headerName: 'Product Price Id', field: 'productPriceId', hide: true },

            { headerName: 'Category Parent Name', field: 'categoryParentName', filter: true },
            { headerName: 'Category Name', field: 'categoryName', filter: true },
            { headerName: 'SubCategory Parent Name', field: 'subCategoryParentName', filter: true },
            { headerName: 'SubCategoryName', field: 'subCategoryName', filter: true },
            { headerName: 'ProductTypeName', field: 'productTypeName', filter: true },
            { headerName: 'Product Title', field: 'productTitle', sortable: true, filter: true, type: ['editableColumn'], },
            { headerName: 'Short Description', field: 'shortDescription', type: ['editableColumn'] },
            { headerName: 'Full Description', field: 'fullDescription' },
            { headerName: 'SKU Code', field: 'skuCode', sortable: true, filter: true, type: ['editableColumn'], },
            { headerName: 'Model Number', field: 'modelNumber', sortable: true, filter: true, type: ['editableColumn'], },
            {
                headerName: 'Model Year', field: 'modelYear', sortable: true, filter: true, cellEditor: "numericCellEditor",
                type: ['editableColumn', 'numberColumn'],
            },
            { headerName: 'Brand Short Name', field: 'brandShortName' },
            { headerName: 'Brand Full Name', field: 'brandFullName' },
            { headerName: 'Tag Name', field: 'tagName' },
            { headerName: 'Fabric Name', field: 'fabricName' },
            {
                headerName: 'Stock Quantity', field: 'stockQuantity', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            {
                headerName: 'Min Stock Quantity', field: 'minStockQuantity', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            {
                headerName: 'Low Stock ActivityId', field: 'lowStockActivityId', hide: true, cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            {
                headerName: 'Notify Admin For Quantity Below', field: 'notifyAdminForQuantityBelow', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            {
                headerName: 'Order Minimum Quantity', field: 'orderMinimumQuantity', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            {
                headerName: 'Order Maximum Quantity', field: 'orderMaximumQuantity', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },

            { headerName: 'Quantity Before Update', field: 'quantityBeforeUpdate' },
            { headerName: 'Quantity Updated OnUtc', field: 'quantityUpdatedOnUtc', type: ["Date"] },
            { headerName: 'Allowed Quantities', field: 'allowedQuantities' },

            {
                headerName: 'Product MRP', field: 'productMRP', cellEditor: "numericCellEditor",
                type: ["editableColumn", "Currency"], sortable: true
            },
            {
                headerName: 'Retail Price', field: 'retailPrice', cellEditor: "numericCellEditor",
                type: ["editableColumn", "Currency"], sortable: true
            },
            {
                headerName: 'Selling Price', field: 'sellingPrice', cellEditor: "numericCellEditor",
                type: ["editableColumn", "Currency"], sortable: true
            },
            {
                headerName: 'Total Percent Off', field: 'totalPercentOff', cellEditor: "numericCellEditor",
                type: ["editableColumn", "Percentage Decimal"], sortable: true
            },
            {
                headerName: 'Special Price', field: 'specialPrice', cellEditor: "numericCellEditor",
                type: ["editableColumn", "Currency"], sortable: true
            },
            { headerName: 'Special Price Start Date', field: 'specialPriceStartDate', type: ["Date"] },
            { headerName: 'Special Price End Date', field: 'specialPriceEndDate', type: ["Date"] },
            {
                headerName: 'Inclusive Sales Tax', field: 'inclusiveSalesTax', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforInclusiveSalesTax
            },

            {
                headerName: 'Show On Home Page', field: 'showOnHomePage', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforShowOnHomePage
            },
            {
                headerName: 'Allow Customer Reviews', field: 'allowCustomerReviews', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforAllowCustomerReviews
            },
            {
                headerName: 'Display Stock Availability', field: 'displayStockAvailability', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforDisplayStockAvailability
            },
            {
                headerName: 'Display Stock Quantity', field: 'displayStockQuantity', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforDisplayStockQuantity
            },

            {
                headerName: 'Is Out Of Stock', field: 'isOutOfStock', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsOutOfStock
            },
            {
                headerName: 'Disable BuyButton', field: 'disableBuyButton', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforDisableBuyButton
            },
            {
                headerName: 'Disable Wishlist Button', field: 'disableWishlistButton', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforDisableWishlistButton
            },
            {
                headerName: 'Has Discounts Applied', field: 'hasDiscountsApplied', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforHasDiscountsApplied
            },
            {
                headerName: 'Published', field: 'published', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforPublished
            },
            {
                headerName: 'Is Recommended', field: 'isRecommended', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsRecommended
            },
            {
                headerName: 'Is Featured', field: 'isFeatured', sortable: true, filter: true,
                type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
                cellEditorParams: this.fillcombostatus, valueGetter: this.lookUpValueGetterforIsFeatured
            },
            {
                headerName: 'Display Order', field: 'displayOrder', cellEditor: "numericCellEditor",
                type: ["editableColumn", "numberColumn"], sortable: true
            },
            { headerName: 'Meta Title', field: 'metaTitle' },
            { headerName: 'Meta Keyword', field: 'metaKeyword' },

            { headerName: 'Approved Rating Sum', field: 'approvedRatingSum' },
            { headerName: 'Not Approved Rating Sum', field: 'notApprovedRatingSum' },
            { headerName: 'Approved Total Reviews', field: 'approvedTotalReviews' },
            { headerName: 'Not Approved Total Reviews', field: 'notApprovedTotalReviews' },
            { headerName: 'Admin Comment', field: 'adminComment' },
            {
                headerName: 'Is Deleted', field: 'isDeleted', sortable: true,
            },
            { headerName: 'Deleted On', field: 'deletedOn', type: ["Date"] },
            { headerName: 'Created OnUtc', field: 'createdOnUtc', type: ["Date"] },
            { headerName: 'Updated OnUtc', field: 'updatedOnUtc', type: ["Date"] },
            { headerName: 'message', field: 'message', type: ["Error"] }
        ];
        this.gridOptions.defaultColDef = { resizable: true };
        this.gridOptions.components = {
            numericCellEditor: getNumericCellEditor()
        };
    }
    //#endregion

    fillcombostatus(param) {
        return {
            values: [true, false]
        }
    }

    lookUpValueGetterforShowOnHomePage(params) {
        if (params.data !== undefined)
            return params.data.showOnHomePage;
    }

    lookUpValueGetterforAllowCustomerReviews(params) {
        if (params.data !== undefined)
            return params.data.allowCustomerReviews;
    }
    lookUpValueGetterforDisplayStockAvailability(params) {
        if (params.data !== undefined)
            return params.data.displayStockAvailability;
    }
    lookUpValueGetterforDisplayStockQuantity(params) {
        if (params.data !== undefined)
            return params.data.displayStockQuantity;
    }
    lookUpValueGetterforIsOutOfStock(params) {
        if (params.data !== undefined)
            return params.data.isOutOfStock;
    }
    lookUpValueGetterforDisableBuyButton(params) {
        if (params.data !== undefined)
            return params.data.disableBuyButton;
    }
    lookUpValueGetterforDisableWishlistButton(params) {
        if (params.data !== undefined)
            return params.data.disableWishlistButton;
    }
    lookUpValueGetterforHasDiscountsApplied(params) {
        if (params.data !== undefined)
            return params.data.hasDiscountsApplied;
    }
    lookUpValueGetterforPublished(params) {
        if (params.data !== undefined)
            return params.data.published;
    }
    lookUpValueGetterforInclusiveSalesTax(params) {
        if (params.data !== undefined)
            return params.data.inclusiveSalesTax;
    }
    lookUpValueGetterforIsDeleted(params) {
        if (params.data !== undefined)
            return params.data.isDeleted;
    }
    lookUpValueGetterforIsRecommended(params) {
        if (params.data !== undefined)
            return params.data.isRecommended;
    }
    lookUpValueGetterforIsFeatured(params) {
        if (params.data !== undefined)
            return params.data.isFeatured;
    }
}
