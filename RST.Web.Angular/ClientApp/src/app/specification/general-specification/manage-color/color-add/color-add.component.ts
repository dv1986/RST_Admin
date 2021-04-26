import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SpecificationService } from '../../../specification.service';

@Component({
    selector: 'app-color-add',
    templateUrl: './color-add.component.html'
})
export class ColorAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        colorName: null,
        colorCodeRGB: null,
        colorCodeHex: null
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _specificationService: SpecificationService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Color";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'colorName': new FormControl('', Validators.required),
            'colorCodeRGB': new FormControl('', Validators.required),
            'colorCodeHex': new FormControl('', Validators.required)
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
        this.model.colorName = this.form.controls['colorName'].value;
        this.model.colorCodeRGB = this.form.controls['colorCodeRGB'].value;
        this.model.colorCodeHex = this.form.controls['colorCodeHex'].value;
        this._specificationService.addColors(this.model).subscribe(result => {
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
