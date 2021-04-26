import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';
import { AttributeService } from '../../attributes.service';

@Component({
    selector: 'app-product-attribute-parent-add',
    templateUrl: './product-attribute-parent-add.component.html'
})
export class ProductAttributeParentAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        name: null,
        description: null
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private _attributeService: AttributeService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Parent Attribute";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'name': new FormControl('', Validators.required),
            'description': new FormControl('')
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
        this.model.name = this.form.controls['name'].value;
        this.model.description = this.form.controls['description'].value;
        this._attributeService.addproductAttributeParent(this.model).subscribe(result => {
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
