import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { AttributeService } from '../../attributes.service';

@Component({
    selector: 'app-product-attribute-add',
    templateUrl: './product-attribute-add.component.html'
})
export class ProductAttributeAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        productAttributeParentId: null,
        attributeName: null
    };

    parentAttributes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _productService: ProductService,
        private _attributeService: AttributeService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Product Feature Category";
        this.initForm();
        this.bindAttributeParentDropDown();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'productAttributeParentId': new FormControl('', Validators.required),
            'attributeName': new FormControl('', Validators.required)
        })
    }

    bindAttributeParentDropDown() {
        this._attributeService.getproductAttributeParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentAttributes = response.data;
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
        this.model.productAttributeParentId = this.form.controls['productAttributeParentId'].value;
        this.model.attributeName = this.form.controls['attributeName'].value;
        this._attributeService.addproductAttribute(this.model).subscribe(result => {
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
