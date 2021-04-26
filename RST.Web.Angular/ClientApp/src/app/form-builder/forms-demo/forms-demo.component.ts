import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilderService } from '../form-builder.service';
import { DndDropEvent, DropEffect } from 'ngx-drag-drop';
import { field, value } from '../global.model';
import { ActivatedRoute } from '@angular/router';
import swal from 'sweetalert2';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MasterService } from 'src/app/master/master.service';

@Component({
  selector: 'forms-demo',
  templateUrl: './forms-demo.component.html'
})
export class FormsDemoComponent implements OnInit {

  modelFields: Array<any> = [];
  public modelOptions: any;

  constructor(private route: ActivatedRoute, private notifictions: NotificationService,
    private _masterService: MasterService, private _formBuilderService: FormBuilderService) {
  }

  ngOnInit() {
    this.initialization();
  }


  initialization() {
    this.modelOptions = {
      name: 'App name...',
      attributes: [],
      dynamicForm: FormGroup,
      submitted: false,
      context: { parentComponent: this },
    };

    if (localStorage.getItem("FormBuilderData") != undefined &&
      localStorage.getItem("FormBuilderData") != null) {
      let data = JSON.parse(localStorage.getItem("FormBuilderData"));
      this.modelFields = JSON.parse(data.formJson);
      this.modelOptions.rowId = data.rowId;
      this.modelOptions.name = data.formName;
      this.modelOptions.attributes = this.modelFields;
      this.initializeForm();
      this.bindCountryDropDown();
    }

    // this._formBuilderService.getFormBuilder(1002).subscribe(result => {
    //   if (result.state == 0) {
    //     this.modelOptions.name = result.data[0].formName;
    //     this.modelFields = JSON.parse(result.data[0].formJson);
    //     this.modelOptions.attributes = this.modelFields;
    //     this.initializeForm();
    //     this.bindCountryDropDown();
    //   }
    // });
  }

  initializeForm() {
    const controls = {};
    let Fields = this.modelFields;
    console.log(Fields);

    // Validation Section
    Fields.forEach(res => {
      if (res.validations != undefined) {
        const validationsArray = [];
        res.validations.forEach(val => {
          if (val.name.toLowerCase() === 'required') {
            validationsArray.push(
              Validators.required
            );
          }
          if (val.name.toLowerCase() === 'pattern') {
            validationsArray.push(
              Validators.pattern(val.validator)
            );
          }
        });

        // add events here for controls according to their formControlName
        if (res.formControlName.toLowerCase() == "country") {
          res.onChange = function (params) { params.context.parentComponent.onCountryChange(params) };
        }

        if (res.formControlName.toLowerCase() == "state") {
          res.onChange = function (params) { params.context.parentComponent.onStateChange(params) };
        }

        // creating form controls here
        controls[res.formControlName] = new FormControl('', validationsArray);
      }
      else {
        // Add button events here
        if (res.formControlName.toLowerCase() == "submit") {
          res.onChange = function (params) { params.context.parentComponent.onSubmit(params) };
        }
      }
    });

    // Form group is generating here
    this.modelOptions.dynamicForm = new FormGroup(
      controls
    );
    console.log(this.modelOptions.dynamicForm);
  }


  fileToUpload: File = null;
  handleFileInput(files) {
    console.log(files);
    this.fileToUpload = files.item(0);
    // this._productService.upload(this.fileToUpload).subscribe(result => {
    //     if (result.state == 0) {
    //         this.notificationService.ShowSuccess("Image uploaded!");
    //         this.modelOptions.productImageId = result.data.rowId;
    //     }
    // });
  }

  countries: any = [];
  states: any = [];
  cities: any = [];
  bindCountryDropDown() {
    // let field = this.modelOptions.fieldsApi.GetFieldById("country");
    // console.log(field)
    this._masterService.getCountry("").subscribe(response => {
      if (response.state == 0) {
        this.countries = response.data;
        this.modelFields.find(x => x.formControlName.toLowerCase() == "country").values = this.countries
      }
    });
  }

  onCountryChange(params) {
    let field = params.context.thisComponent.options.fieldsApi.GetFieldById("state");
    this._masterService.getStateLookup(params.value.rowId).subscribe(response => {
      if (response.state == 0) {
        this.states = response.data;
        //this.modelFields.find(x => x.formControlName == "state").values = this.states
        field.values = this.states;
      }
    });
  }

  onStateChange(params) {
    let field = params.context.thisComponent.options.fieldsApi.GetFieldById("city");
    this._masterService.getCityLookup(params.value.rowId).subscribe(response => {
      if (response.state == 0) {
        this.cities = response.data;
        // this.modelFields.find(x => x.formControlName.toLowerCase() == "city").values = this.cities
        field.values = this.cities;
      }
    });
  }

  onSubmit() {
    this.modelOptions.submitted = true;
    // stop here if form is invalid
    if (this.modelOptions.dynamicForm.invalid) {
      return;
    }
    console.log(this.modelOptions.dynamicForm.value);
  }
}




  // initialize() {
  //   this.modelFields = [
  //     {
  //       "type": "text",
  //       "icon": "fa-font",
  //       "label": "UserName",
  //       "description": "Enter your name",
  //       "placeholder": "Enter your name",
  //       "className": "form-control",
  //       "subtype": "text",
  //       "regex": "",
  //       "handle": true,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "UserName is required"
  //         }
  //       ],
  //       "col": 6,
  //       "name": "text-1609308516211",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "text",
  //       "icon": "fa-font",
  //       "label": "Email",
  //       "description": "Enter your name",
  //       "placeholder": "Enter your name",
  //       "className": "form-control",
  //       "subtype": "text",
  //       "regex": "^([a-zA-Z0-9_.-]+)@([a-zA-Z0-9_.-]+)\\.([a-zA-Z]{2,5})$",
  //       "handle": true,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Email is required"
  //         },
  //         {
  //           "name": "pattern",
  //           "validator": "^([a-zA-Z0-9_.-]+)@([a-zA-Z0-9_.-]+)\\.([a-zA-Z]{2,5})$",
  //           "message": "Please enter a valid email"
  //         }
  //       ],
  //       "col": 6,
  //       "name": "text-1609308533773",
  //       "toggle": false,
  //       "required": true,
  //       "errorText": "Please enter a valid email"
  //     },
  //     {
  //       "type": "number",
  //       "label": "Number",
  //       "icon": "fa-html5",
  //       "description": "Age",
  //       "placeholder": "Enter your age",
  //       "className": "form-control",
  //       "value": "20",
  //       "min": 12,
  //       "max": 90,
  //       "col": 6,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Number is required."
  //         }
  //       ],
  //       "name": "number-1609309268140",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "date",
  //       "icon": "fa-calendar",
  //       "label": "Date",
  //       "subtype": "date",
  //       "placeholder": "Date",
  //       "className": "form-control",
  //       "col": 6,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Date is required."
  //         }
  //       ],
  //       "name": "date-1609309499785",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "datetime-local",
  //       "icon": "fa-calendar",
  //       "label": "DateTime",
  //       "subtype": "datetime-local",
  //       "placeholder": "Date Time",
  //       "className": "form-control",
  //       "col": 6,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "DateTime is required."
  //         }
  //       ],
  //       "name": "datetime-local-1609309503047",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "textarea",
  //       "icon": "fa-text-width",
  //       "label": "Textarea",
  //       "subtype": "textarea",
  //       "col": 12,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Textarea is required."
  //         }
  //       ],
  //       "name": "textarea-1609309668834",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "autocomplete",
  //       "icon": "fa-bars",
  //       "label": "Country",
  //       "description": "Select",
  //       "placeholder": "Select",
  //       "className": "form-control",
  //       "bindLabel": "countryName",
  //       "bindValue": "rowId",
  //       "col": 6,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Country is required."
  //         }
  //       ],
  //       "name": "autocomplete-1609326603902",
  //       "toggle": false,
  //       "required": true,
  //       "onChange": ""
  //       // onChange: function (params) { params.context.parentComponent.onCountryChange(params) }
  //     },
  //     {
  //       "type": "autocomplete",
  //       "icon": "fa-bars",
  //       "label": "State",
  //       "description": "Select",
  //       "placeholder": "Select",
  //       "className": "form-control",
  //       "bindLabel": "stateName",
  //       "bindValue": "rowId",
  //       "col": 6,
  //       "validations": [
  //         {
  //           "name": "required",
  //           "validator": "required",
  //           "message": "Country is required."
  //         }
  //       ],
  //       "name": "autocomplete-1609326603901",
  //       "toggle": false,
  //       "required": true
  //     },
  //     {
  //       "type": "submit",
  //       "icon": "fa-paper-plane",
  //       "subtype": "submit",
  //       "label": "Submit",
  //       "col": 6,
  //       "name": "btnSubmit-1609310689103",
  //       "toggle": false,
  //       "onChange": ""
  //       //"onChange": function (params) { params.context.parentComponent.onSubmit(params) }
  //     },
  //     {
  //       "type": "reset",
  //       "icon": "fa-paper-plane",
  //       "subtype": "reset",
  //       "label": "Cancel",
  //       "col": 6,
  //       "name": "reset-1609311789210"
  //     }
  //   ]
  //   this.modelOptions.attributes = this.modelFields;
  // }
