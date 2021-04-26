import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterService } from '../../master.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-seo-content-add',
    templateUrl: './seo-content-add.component.html'
})
export class SeoContentAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        metaTitle: null,
        metaKeyword: null,
        metaDescription: null
    };
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _masterService: MasterService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New SEO Content";
        this.initForm();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'metaTitle': new FormControl('', Validators.required),
            'metaKeyword': new FormControl('', Validators.required),
            'metaDescription': new FormControl('', Validators.required)
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
        this.model.metaTitle = this.form.controls['metaTitle'].value;
        this.model.metaKeyword = this.form.controls['metaKeyword'].value;
        this.model.metaDescription = this.form.controls['metaDescription'].value;
        this._masterService.addSeoContent(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                // this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }


    onCancel(): void {
        this.form.controls['metaTitle'].setValue('');
        this.form.controls['metaKeyword'].setValue('');
        this.form.controls['metaDescription'].setValue('');
        this.bsModalRef.hide();
    }
}
