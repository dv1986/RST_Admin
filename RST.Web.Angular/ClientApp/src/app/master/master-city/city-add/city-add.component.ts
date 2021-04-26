import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterService } from '../../master.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
    selector: 'app-city-add',
    templateUrl: './city-add.component.html'
})
export class CityAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        countryId: null,
        stateId: null,
        cityName: null
    };
    countries: any = [];
    states: any = [];

    @Output() onSuccess = new EventEmitter<any>();

    constructor(private _masterService: MasterService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New City";
        this.initForm();
        this.bindCountryDropDown();
    }

    bindCountryDropDown() {
        this._masterService.getCountry("").subscribe(response => {
            if (response.state == 0) {
                this.countries = response.data;
            }
        });
    }

    onCountryChange(params) {
        this._masterService.getStateLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.states = response.data;
            }
        });
    }



    initForm() {
        this.form = new FormGroup({
            'countryId': new FormControl('', Validators.required),
            'stateId': new FormControl('', Validators.required),
            'cityName': new FormControl('', Validators.required)
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
        this.model.countryId = this.form.controls['countryId'].value;
        this.model.stateId = this.form.controls['stateId'].value;
        this.model.cityName = this.form.controls['cityName'].value;
        this._masterService.addCity(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
            }
        });
    }


    onCancel(): void {
        this.form.controls['countryId'].setValue('');
        this.form.controls['stateId'].setValue('');
        this.form.controls['cityName'].setValue('');
    }
}
