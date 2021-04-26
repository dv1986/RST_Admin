import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterService } from '../../master.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-state-add',
    templateUrl: './state-add.component.html'
})
export class StateAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        countryId: null,
        stateName: null
    };
    public onAdd: Subject<boolean>;
    countries: any = [];

    @Output() onSuccess = new EventEmitter<any>();



    constructor(private _masterService: MasterService,
        public bsModalRef: BsModalRef,
        private lookupService: MasterService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New State";
        this.initForm();
        this.bindCountryDropDown();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'countryId': new FormControl('', Validators.required),
            'stateName': new FormControl('', Validators.required)
        })
    }

    bindCountryDropDown() {
        this.lookupService.getCountry("").subscribe(response => {
            if (response.state == 0) {
                this.countries = response.data;
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
        this.model.countryId = this.form.controls['countryId'].value;
        this.model.stateName = this.form.controls['stateName'].value;
        this._masterService.addState(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.form.controls['stateName'].setValue('');
        this.bsModalRef.hide();
    }
}
