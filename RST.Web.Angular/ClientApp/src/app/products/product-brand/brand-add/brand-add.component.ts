import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ProductService } from '../../product.service';

@Component({
    selector: 'app-brand-add',
    templateUrl: './brand-add.component.html'
})
export class BrandAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        shortName: null,
        fullName: null,
        description: null,
        productImageId: null,
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Brand";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'shortName': new FormControl('', Validators.required),
            'fullName': new FormControl('', Validators.required),
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
        this.model.shortName = this.form.controls['shortName'].value;
        this.model.fullName = this.form.controls['fullName'].value;
        this.model.description = this.form.controls['description'].value;
        this._productService.addBrands(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }

    fileToUpload: File = null;
    handleFileInput(files) {
        console.log(files);
        this.fileToUpload = files.item(0);
        this._productService.upload(this.fileToUpload).subscribe(result => {
            if (result.state == 0) {
                this.notificationService.ShowSuccess("Image uploaded!");
                this.model.productImageId = result.data.rowId;
            }
        });
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
