import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { AttributeService } from '../../attributes.service';

@Component({
    selector: 'app-category-attribute-mapping-edit',
    templateUrl: './category-attribute-mapping-edit.component.html'
})
export class CategoryAttributeMappingEditComponent implements OnInit {

    productTypeId = null;
    categoryParentId = null;
    categoryId = null;
    subCategoryParentId = null;
    subCategoryId = null;
    form: FormGroup;
    pageTitle: string;
    isEditMode = false;
    model: any = {
        productTypeId: null,
        categoryParentId: null,
        categoryId: null,
        subCategoryParentId: null,
        subCategoryId: null,
        attributeIds: [],
    };

    parentAttributes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _attributeService: AttributeService,
        private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Update Attributes";
        this.initForm();
        this.bindAttributes();
        this.onAdd = new Subject();
        if (this.productTypeId != undefined && this.productTypeId != null) {
            this.loadData();
            this.isEditMode = true;
        }
    }

    bindAttributes() {
        this._attributeService.getproductAttributeParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentAttributes = response.data;
            }
        });
    }

    loadData() {
        this._attributeService.getAttributeProductTypeMapping(
            this.categoryParentId,
            this.categoryId,
            this.subCategoryParentId,
            this.subCategoryId,
            this.productTypeId).subscribe(response => {
                if (response.state == 0)
                    if (response.data != null && response.data.length > 0) {
                        for (let index = 0; index < response.data.length; index++) {
                            this.model.attributeIds.push(response.data[index].productAttributeId);
                        }
                        this.form.controls['attributeIds'].setValue(this.model.attributeIds);
                        console.log(this.model.attributeIds);
                    }
            });
    }

    initForm() {
        this.form = new FormGroup({
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
        this.model.categoryParentId = this.categoryParentId == 0 ? null : this.categoryParentId;
        this.model.categoryId = this.categoryId == 0 ? null : this.categoryId;
        this.model.subCategoryParentId = this.subCategoryParentId == 0 ? null : this.subCategoryParentId;
        this.model.subCategoryId = this.subCategoryId == 0 ? null : this.subCategoryId;
        this.model.productTypeId = this.productTypeId == 0 ? null : this.productTypeId;
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
