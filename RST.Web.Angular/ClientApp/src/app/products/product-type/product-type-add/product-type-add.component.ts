import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';
import { SpecificationService } from 'src/app/specification/specification.service';

@Component({
    selector: 'app-product-type-add',
    templateUrl: './product-type-add.component.html'
})
export class ProductTypeAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        productSubCategoryId: null,
        productCategoryId: null,
        productSubCategoryParentId: null,    
        name: null,
        description: null,
        seoContentId: null,
        productImageId: null,
        displayOrder: null,
        includeInTopMenu: null,
        isNew: null,
        hasDiscountApplied: null,
        isPublished: null,
        productSizeTypeId: null
    };

    categories: any = [];
    subParentCategories: any = [];
    subCategories: any = [];
    sizeTypes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _productService: ProductService,
        private _specificationService: SpecificationService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Parent Sub-Category";
        this.initForm();
        this.bindCategoryDropDown();
        this.bindProdcutSizeType();
        this.onAdd = new Subject();
    }

    bindCategoryDropDown() {
        this._productService.getProductCategory("").subscribe(response => {
            if (response.state == 0) {
                this.categories = response.data;
            }
        });
    }

    onCategoryChange(params) {
        this._productService.getSubCategoryParentLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.subParentCategories = response.data;
            }
        });
    }

    onSubCategoryParentChange(params) {
        this._productService.getSubCategoryLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.subCategories = response.data;
            }
        });
    }

    bindProdcutSizeType() {
        this._specificationService.getProductSizeType("").subscribe(response => {
            if (response.state == 0) {
                this.sizeTypes = response.data;
            }
        });
    }

    initForm() {
        this.form = new FormGroup({
            'productSubCategoryId': new FormControl('', Validators.required),
            'productCategoryId': new FormControl('', Validators.required),
            'productSubCategoryParentId': new FormControl('', Validators.required),
            'name': new FormControl('', Validators.required),
            'description': new FormControl(''),
            'seoContentId': new FormControl(0),
            'displayOrder': new FormControl(0),
            'includeInTopMenu': new FormControl(false),
            'isNew': new FormControl(false),
            'hasDiscountApplied': new FormControl(false),
            'isPublished': new FormControl(false),
            'productSizeTypeId': new FormControl('', Validators.required),
        })
    }


   // convenience getter for easy access to form fields
   get f() { return this.form.controls; }
   submitted = false;
   onSubmit() {

       this.submitted = true;
       // stop here if form is invalid
       if (this.form.invalid) {
           return;
       }
        this.model.rowId = 0;
        this.model.productSubCategoryId = this.form.controls['productSubCategoryId'].value;
        this.model.productCategoryId = this.form.controls['productCategoryId'].value;
        this.model.productSubCategoryParentId = this.form.controls['productSubCategoryParentId'].value;
        this.model.name = this.form.controls['name'].value;
        this.model.description = this.form.controls['description'].value;
        this.model.seoContentId = this.form.controls['seoContentId'].value;
        this.model.displayOrder = this.form.controls['displayOrder'].value.toString();
        this.model.includeInTopMenu = this.form.controls['includeInTopMenu'].value;
        this.model.isNew = this.form.controls['isNew'].value;
        this.model.hasDiscountApplied = this.form.controls['hasDiscountApplied'].value;
        this.model.isPublished = this.form.controls['isPublished'].value;
        this.model.productSizeTypeId = this.form.controls['productSizeTypeId'].value;
        if (this.model.productImageId == null)
            this.model.productImageId = 0;
        this._productService.addProductType(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
