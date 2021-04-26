import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GenericValidator } from 'src/app/shared/generic-validator';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SpecificationService } from '../../../specification.service';

@Component({
    selector: 'app-measure-dimension-add',
    templateUrl: './measure-dimension-add.component.html'
})
export class MeasureDimensionAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        name: null,
        systemKeyword: null,
        ratio: null,
        displayOrder:null
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _specificationService: SpecificationService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Measure Dimension";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'name': new FormControl('', Validators.required),
            'systemKeyword': new FormControl('', Validators.required),
            'ratio': new FormControl('', Validators.required),
            'displayOrder': new FormControl('', Validators.required)
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
        this.model.systemKeyword = this.form.controls['systemKeyword'].value;
        this.model.ratio = this.form.controls['ratio'].value.toString();
        this.model.displayOrder = this.form.controls['displayOrder'].value.toString();
        this._specificationService.addMeasureDimension(this.model).subscribe(result => {
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
