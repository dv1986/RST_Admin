import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterService } from '../../master.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-submenu-add',
    templateUrl: './submenu-add.component.html'
})
export class SubMenuAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        menuId: null,
        subMenuName: null
    };
    public onAdd: Subject<boolean>;
    menues: any = [];

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _masterService: MasterService,
        public bsModalRef: BsModalRef,
        private lookupService: MasterService,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Sub-Menu";
        this.initForm();
        this.bindMenuDropDown();
        this.onAdd = new Subject();
    }

    initForm() {
        this.form = new FormGroup({
            'menuId': new FormControl('', Validators.required),
            'subMenuName': new FormControl('', Validators.required)
        })
    }

    bindMenuDropDown() {
        this.lookupService.getMenu("").subscribe(response => {
            if (response.state == 0) {
                this.menues = response.data;
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
        this.model.menuId = this.form.controls['menuId'].value;
        this.model.subMenuName = this.form.controls['subMenuName'].value;
        this._masterService.addSubMenu(this.model).subscribe(result => {
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
