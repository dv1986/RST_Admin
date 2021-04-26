import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GenericValidator } from 'src/app/shared/generic-validator';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';

@Component({
    selector: 'app-type-add',
    templateUrl: './type-add.component.html'
})
export class TypeAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        categoryName: null,
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();

    // Use with the generic validation message class
    displayMessage: { [key: string]: string } = {};
    private validationMessages: { [key: string]: { [key: string]: string } };
    private genericValidator: GenericValidator;

    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
        this.validation();
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Product Type";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'typeName': new FormControl('', Validators.required)
        })
    }


    onSubmit() {
        this.model.rowId = 0;
        this.model.typeName = this.form.controls['typeName'].value;
        this._productService.addProductType(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.form.controls['typeName'].setValue('');
        this.bsModalRef.hide();
    }

    validation() {
        // Defines all of the validation messages for the form.
        // These could instead be retrieved from a file or database.
        this.validationMessages = {
            typeName: {
                required: 'type Name is required.'
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
