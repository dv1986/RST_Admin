import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GenericValidator } from 'src/app/shared/generic-validator';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { AttributeService } from 'src/app/attributes/attributes.service';

@Component({
    selector: 'app-product-attribute-mapping-add',
    templateUrl: './product-attribute-mapping-add.component.html'
})
export class ProductAttributeMappingAddComponent implements OnInit {

    productId = null;
    form: FormGroup;
    pageTitle: string;
    model: any = {
        productId: null,
        productAttributes: []
    };

    parentAttributes: any = [];
    attributes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();

    // Use with the generic validation message class
    displayMessage: { [key: string]: string } = {};
    private validationMessages: { [key: string]: { [key: string]: string } };
    private genericValidator: GenericValidator;

    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private _attributeService: AttributeService,
        private notificationService: NotificationService) {
        this.validation();
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Product Feature Category";
        this.initForm();
        this.bindParentAttributes();
        this.bindAttributes();
        this.loadData();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'productAttributes': new FormControl(null),
        })
    }

    bindParentAttributes() {
        this._attributeService.getproductAttributeParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentAttributes = response.data;
            }
        });
    }

    bindAttributes() {
        this._attributeService.getproductAttribute("").subscribe(response => {
            if (response.state == 0) {
                this.attributes = response.data;
            }
        });
    }

    onParentAttributeChange(params) {
        this._attributeService.getproductAttribute(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.attributes = response.data;
            }
        });
    }

    loadData() {
        this._productService.getProductAttributeMapping(this.productId).subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    console.log(response.data);
                    for (let index = 0; index < response.data.length; index++) {
                        this.model.productAttributes.push(response.data[index].productAttributeId);
                    }
                    this.form.controls['productAttributes'].setValue(this.model.productAttributes);
                    console.log(this.model.productAttributes);
                }
        });
    }


    onSubmit() {
        //this.model.rowId = 0;
        this.model.productAttributes = this.form.controls['productAttributes'].value;
        this.model.productId = this.productId;
        console.log(this.model.productAttributes);
        this._productService.updateProductAttributeProductList(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.form.controls['productAttributes'].setValue('');
        this.bsModalRef.hide();
    }

    validation() {
        // Defines all of the validation messages for the form.
        // These could instead be retrieved from a file or database.
        this.validationMessages = {
            attributeName: {
                required: 'Attribute Name is required.'
            }
        };

        // Define an instance of the validator for use with this form,
        // passing in this form's set of validation messages.
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    // Also validate on blur
    // Helpful if the user tabs through required fields
    blur(): void {
        this.displayMessage = this.genericValidator.processMessages(this.form);
    }
}
