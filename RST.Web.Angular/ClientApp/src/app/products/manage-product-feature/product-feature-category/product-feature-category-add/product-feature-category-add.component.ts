import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from 'src/app/products/product.service';

@Component({
    selector: 'app-product-feature-category-add',
    templateUrl: './product-feature-category-add.component.html'
})
export class ProductFeatureCategoryAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        categoryShortName: null,
        categoryLongName: null,
        categoryDescription: null
    };
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
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'categoryShortName': new FormControl('', Validators.required),
            'categoryLongName': new FormControl('', Validators.required),
            'categoryDescription': new FormControl('')
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
        this.model.categoryShortName = this.form.controls['categoryShortName'].value;
        this.model.categoryLongName = this.form.controls['categoryLongName'].value;
        this.model.categoryDescription = this.form.controls['categoryDescription'].value;
        this._productService.addProductsFeaturesCategory(this.model).subscribe(result => {
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
