import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';
import { ImageViewer } from 'src/app/shared/components/ImageViewer/ImageViewer';
import { Imagecompressionpopup } from 'src/app/shared/components/image-compression-popup/image-compression-popup.component';
import { ProductImageMappingAddComponent } from '../product-images-mapping-add/product-images-mapping-add.component';



@Component({
    selector: 'app-product-images-mapping-list',
    templateUrl: './product-images-mapping-list.component.html'
})
export class ProductImageMappingListComponent implements OnInit {


    dataList: any[] = [];
    public columnDefs: any[];
    public gridOptions: GridOptions = <GridOptions>{};
    public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
    public gridActions: any[];
    // filterComponent: GridFilterItem = new GridFilterItem(CountrySearchComponent, this);

    @Output() onAdd = new EventEmitter<any>();

    constructor(private _productService: ProductService,
        private notificationService: NotificationService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService
    ) {
        this.initGrid();
        this.initGridActions();
    }

    ngOnInit() {

    }

    productId: 0;
    loadData(productId) {
        this.productId = productId;
        this._productService.GetProductImageMapping(productId).subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    this.dataList = response.data;
                }
                else {
                    this.dataList = [];
                }
        });
    }

    // onUpdate() {
    //     if (this.gridInformation.updatedData.length == 0) {
    //         this.notificationService.ShowError("Please select record(s) to update data!", 3000);
    //         return;
    //     }
    //     this._productService.updateProductAttributeProduct(this.gridInformation.updatedData).subscribe(response => {
    //         if (response.state == 0) {
    //             this.onRefresh();
    //             this.notificationService.ShowSuccess("Record updated sucessfully!", 3000);
    //         }
    //         else if (response.state == 4) {
    //             for (let rowindex in response.data) {
    //                 let item = this.dataList.find(fn => fn.rowId == response.data[rowindex].rowId)
    //                 item.message = response.data[rowindex].message;
    //                 if (item.message != "")
    //                     this.notificationService.ShowWarning("Please check Message Column", 3000)
    //             }
    //             this.gridOptions.api.refreshCells({ force: true });
    //         }
    //     });
    // }

    onDelete() {
        if (this.gridInformation.selectedData.length == 0) {
            this.notificationService.ShowError("Please select record(s) to delete data!", 3000);
            return;
        }
        this._productService.deleteProductImageProduct(this.gridInformation.selectedData).subscribe(response => {
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
        this.loadData(this.productId);
        this.gridInformation.selectedData = [];
        this.gridInformation.updatedData = [];
        this.gridOptions.api.deselectAll();
    }

    private modalRef: BsModalRef;
    onAddNew() {
        //this.onAdd.emit();
        var initialState = {
            isModal: true,
            isEditMode: false,
            productId: this.productId
        };
        this.modalRef = this.modalService.show(ProductImageMappingAddComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" })
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
                cssClass: "btn btn-info btn-xs",
                onClick: function (event, context) {
                    context.mainComponent.onAddNew();
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
            { headerName: 'Product Id', field: 'productId', hide: true },
            { headerName: 'Product Id', field: 'productImageId', hide: true },
            { headerName: 'Image', field: 'thumbnailPath', onCellClicked: this.onImageClick, cellRendererFramework: ImageViewer },
            { headerName: 'Image Name', field: 'imageName' },
            { headerName: 'Image Path', field: 'imagePath' },
            { headerName: 'Thumbnail Path', field: 'thumbnailPath'},
        ];
        this.gridOptions.defaultColDef = { resizable: true };
    }
    //#endregion

    private modalRef1: BsModalRef;
    onImageClick(params) {
        debugger;
        const initialState = {
            ImagePath: params.data.imagePath,
            ImageName: params.data.imageName,
            moduleName: "product",
            rowId: params.data.rowId
        };
        params.context["mainComponent"].ShowImageModalRef = params.context["mainComponent"].modalService.show(Imagecompressionpopup, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" })
        if (params.context["mainComponent"].ShowImageModalRef) {
            params.context["mainComponent"].ShowImageModalRef.content.onAdd.subscribe(result => {
                params.context["mainComponent"].onRefresh();
            })
        }
    }
}
