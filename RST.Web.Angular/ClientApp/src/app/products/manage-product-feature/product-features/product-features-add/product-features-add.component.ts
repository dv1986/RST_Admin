import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';

@Component({
    selector: 'app-product-features-add',
    templateUrl: './product-features-add.component.html'
})
export class ProductFeaturesAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        productFeatureCategoryId: null,
        featureName: null,
        description: null
    };

    featureCategories: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();

    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Product Feature Category";
        this.initForm();
        this.bindFeatureCategoryDropDown();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'productFeatureCategoryId': new FormControl('', Validators.required),
            'featureName': new FormControl('', Validators.required),
            'description': new FormControl('')
        })
    }

    bindFeatureCategoryDropDown() {
        this._productService.getProductsFeaturesCategory("").subscribe(response => {
            if (response.state == 0) {
                this.featureCategories = response.data;
            }
        });
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
        this.model.productFeatureCategoryId = this.form.controls['productFeatureCategoryId'].value;
        this.model.featureName = this.form.controls['featureName'].value;
        this.model.description = this.form.controls['description'].value;
        this._productService.addProductFeatures(this.model).subscribe(result => {
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
