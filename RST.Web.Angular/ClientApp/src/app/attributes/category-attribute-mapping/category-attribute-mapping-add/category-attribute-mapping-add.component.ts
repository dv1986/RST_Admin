import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { AttributeService } from '../../attributes.service';

@Component({
    selector: 'app-category-attribute-mapping-add',
    templateUrl: './category-attribute-mapping-add.component.html'
})
export class CategoryAttributeMappingAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        categoryParentId: null,
        categoryId: null,
        subCategoryParentId: null,
        subCategoryId: null,
        productTypeId: null,
        attributeIds: [],
    };

    parentAttributes: any = [];
    parentCategories: any = [];
    categories: any = [];
    subParentCategories: any = [];
    subCategories: any = [];
    productTypes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _attributeService: AttributeService,
        private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Product Feature Category";
        this.initForm();
        this.bindParentCategoryDropDown();
        this.bindAttributes();
        this.onAdd = new Subject();
    }

    bindParentCategoryDropDown() {
        this._productService.getProductCategoryParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentCategories = response.data;
            }
        });
    }

    onCategoryParentChange(params) {
        this._productService.GetCategoryLookup(params.rowId).subscribe(response => {
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

    onSubCategoryChange(params) {
        this._productService.GetProductTypeLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.productTypes = response.data;
            }
        });
    }

    bindAttributes() {
        this._attributeService.getproductAttributeParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentAttributes = response.data;
            }
        });
    }

    initForm() {
        this.form = new FormGroup({
            'categoryParentId': new FormControl(null, Validators.required),
            'categoryId': new FormControl(null),
            'subCategoryParentId': new FormControl(null),
            'subCategoryId': new FormControl(null),
            'productTypeId': new FormControl(null),
            'attributeIds': new FormControl('', Validators.required)
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
        this.model.categoryParentId = this.form.controls['categoryParentId'].value;
        this.model.categoryId = this.form.controls['categoryId'].value;
        this.model.subCategoryParentId = this.form.controls['subCategoryParentId'].value;
        this.model.subCategoryId = this.form.controls['subCategoryId'].value;
        this.model.productTypeId = this.form.controls['productTypeId'].value;
        this.model.attributeIds = this.form.controls['attributeIds'].value;
        this._attributeService.addAttributeProductTypeMapping(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
