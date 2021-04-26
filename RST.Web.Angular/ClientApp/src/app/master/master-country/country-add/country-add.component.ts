import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterService } from '../../master.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-country-add',
    templateUrl: './country-add.component.html'
})
export class CountryAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        countryName: null,
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _masterService: MasterService,
        public bsModalRef: BsModalRef,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Country";
        this.initForm();
        this.onAdd = new Subject();
    }



    initForm() {
        this.form = new FormGroup({
            'countryName': new FormControl('', Validators.required)
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
        this.model.countryName = this.form.controls['countryName'].value;
        this._masterService.addCountry(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
