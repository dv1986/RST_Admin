import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { SpecificationService } from '../../../specification.service';

@Component({
    selector: 'app-size-type-add',
    templateUrl: './size-type-add.component.html'
})
export class SizeTypeAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        typeName: null,
        typeCode: null,
        measureDimensionId: null,
        description:null
    };
    public onAdd: Subject<boolean>;
    measurementDimensionList: any = [];

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _specificationService: SpecificationService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Size Type";
        this.initForm();
        this.bindMeasurementDimensionDropDown();
        this.onAdd = new Subject();
    }

    bindMeasurementDimensionDropDown() {
        this._specificationService.getMeasureDimension("").subscribe(response => {
            if (response.state == 0) {
                this.measurementDimensionList = response.data;
            }
        });
    }

    initForm() {
        this.form = new FormGroup({
            'typeName': new FormControl('', Validators.required),
            'typeCode': new FormControl('', Validators.required),
            'measureDimensionId': new FormControl('', Validators.required),
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
        this.model.typeName = this.form.controls['typeName'].value;
        this.model.typeCode = this.form.controls['typeCode'].value;
        this.model.measureDimensionId = this.form.controls['measureDimensionId'].value;
        this.model.description = this.form.controls['description'].value;
        this._specificationService.addProductSizeType(this.model).subscribe(result => {
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
