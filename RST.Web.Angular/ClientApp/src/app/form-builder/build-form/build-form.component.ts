import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilderService } from '../form-builder.service';
import { DndDropEvent, DropEffect } from 'ngx-drag-drop';
import { field, value } from '../global.model';
import { ActivatedRoute } from '@angular/router';
import swal from 'sweetalert2';
import { FormGroup } from '@angular/forms';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
    selector: 'build-form',
    templateUrl: './build-form.component.html',
    styleUrls: ['./build-form.component.css']
})
export class BuildFormComponent implements OnInit {

    show = 0
    eventName = "Save";
    @Output() onSuccess = new EventEmitter<any>();
    value: value = {
        label: "",
        value: ""
    };

    validations: any = {
        name: "",
        validator: "",
        message: ""
    };

    success = false;

    fieldModels: Array<any> = [
        {
            "type": "text",
            "icon": "fa-font",
            "label": "Text",
            "formControlName": "Text",
            "description": "Enter your name",
            "placeholder": "Enter your name",
            "className": "form-control",
            "subtype": "text",
            "regex": "",
            "handle": true,
            "col": 6,
            "disabled": false,
            "validations": [

            ]
        },
        // {
        //     "type": "email",
        //     "icon": "fa-envelope",
        //     "required": true,
        //     "label": "Email",
        //     "description": "Enter your email",
        //     "placeholder": "Enter your email",
        //     "className": "form-control",
        //     "subtype": "text",
        //     "regex": "^([a-zA-Z0-9_.-]+)@([a-zA-Z0-9_.-]+)\.([a-zA-Z]{2,5})$",
        //     "errorText": "Please enter a valid email",
        //     "handle": true,
        //     "col": 6
        // },
        // {
        //     "type": "phone",
        //     "icon": "fa-phone",
        //     "label": "Phone",
        //     "description": "Enter your phone",
        //     "placeholder": "Enter your phone",
        //     "className": "form-control",
        //     "subtype": "text",
        //     "regex": "^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$",
        //     "errorText": "Please enter a valid phone number",
        //     "handle": true,
        //     "col": 6
        // },
        {
            "type": "number",
            "label": "Number",
            "formControlName": "Number",
            "icon": "fa-html5",
            "description": "Age",
            "placeholder": "Enter your age",
            "className": "form-control",
            "subtype": "number",
            "value": "20",
            "min": 12,
            "max": 90,
            "col": 6,
            "disabled": false,
            "validations": [

            ]
        },
        {
            "type": "date",
            "icon": "fa-calendar",
            "label": "Date",
            "formControlName": "Date",
            "subtype": "date",
            "placeholder": "Date",
            "className": "form-control",
            "col": 6,
            "disabled": false,
            "validations": [

            ]
        },
        {
            "type": "datetime-local",
            "icon": "fa-calendar",
            "label": "DateTime",
            "formControlName": "DateTime",
            "subtype": "datetime-local",
            "placeholder": "Date Time",
            "className": "form-control",
            "col": 6,
            "disabled": false,
            "validations": [

            ]
        },
        {
            "type": "textarea",
            "icon": "fa-text-width",
            "label": "Textarea",
            "formControlName": "Textarea",
            "subtype": "textarea",
            "col": 6,
            "validations": [

            ]
        },
        // {
        //     "type": "paragraph",
        //     "icon": "fa-paragraph",
        //     "label": "Paragraph",
        //     "placeholder": "Type your text to display here only",
        //     "col": 12
        // },
        {
            "type": "checkbox",
            "required": true,
            "label": "Checkbox",
            "formControlName": "Checkbox",
            "icon": "fa-list",
            "description": "Checkbox",
            "inline": true,
            "values": [
                {
                    "label": "Option 1",
                    "value": "option-1"
                },
                {
                    "label": "Option 2",
                    "value": "option-2"
                }
            ],
            "col": 6,
            "validations": [

            ]
        },
        {
            "type": "radio",
            "icon": "fa-list-ul",
            "label": "Radio",
            "formControlName": "Radio",
            "description": "Radio boxes",
            "values": [
                {
                    "label": "Option 1",
                    "value": "option-1"
                },
                {
                    "label": "Option 2",
                    "value": "option-2"
                }
            ],
            "col": 6,
            "validations": [

            ]
        },
        {
            "type": "autocomplete",
            "icon": "fa-bars",
            "label": "Select",
            "formControlName": "Select",
            "description": "Select",
            "placeholder": "Select",
            "bindLabel": "",
            "bindValue": "",
            "className": "form-control",
            "values": [
                {
                    "label": "Option 1",
                    "value": "option-1"
                },
                {
                    "label": "Option 2",
                    "value": "option-2"
                },
                {
                    "label": "Option 3",
                    "value": "option-3"
                }
            ],
            "col": 6,
            "disabled": false,
            "validations": [

            ]
        },
        {
            "type": "file",
            "icon": "fa-file",
            "label": "File Upload",
            "formControlName": "fileUpload",
            "className": "form-control",
            "subtype": "file",
            "col": 6
        },
        {
            "type": "submit",
            "icon": "fa-paper-plane",
            "subtype": "button",
            "label": "Submit",
            "formControlName": "Submit",
            "col": 6
        }
        ,
        {
            "type": "reset",
            "icon": "fa-paper-plane",
            "subtype": "reset",
            "label": "Cancel",
            "formControlName": "Cancel",
            "col": 6,
        }
    ];

    modelFields: Array<any> = [];
    model: any = {
        // description: 'App Description...',
        // theme: {
        //     bgColor: "ffffff",
        //     textColor: "555555",
        //     bannerImage: ""
        // },
        rowId: 0,
        name: 'App name...',
        attributes: this.modelFields,
        dynamicForm: FormGroup,
        submitted: false,
        // context: { parentComponent: this },
    };

    report = false;
    reports: any = [];

    addPatternValidation(item) {
        if (item.validations.findIndex(x => x.name == "pattern") != -1)
            item.validations.splice(item.validations.findIndex(x => x.name == "pattern"), 1)

        if (item.regex != undefined && item.regex != "") {
            this.validations = { name: "pattern", validator: item.regex, message: item.errorText == undefined ? "" : item.errorText };
            item.validations.push(this.validations);
        }
    }

    addRequiredValidation(item, flag) {
        if (flag) {
            item.required = true;
            this.validations = { name: "required", validator: "required", message: item.label + " is required." };
            item.validations.push(this.validations);
        }
        else {
            item.required = false;
            item.validations.splice(item.validations.findIndex(x => x.name == "required"), 1)
        }
    }

    constructor(
        private route: ActivatedRoute,
        private _formBuilderService: FormBuilderService,
        private notificationService: NotificationService
    ) { }

    ngOnInit() {
        this.initialize();
    }

    initialize() {
        if (localStorage.getItem("FormBuilderData") != undefined &&
            localStorage.getItem("FormBuilderData") != null) {
            let data = JSON.parse(localStorage.getItem("FormBuilderData"));
            this.modelFields = JSON.parse(data.formJson);

            this.model.rowId = data.rowId;
            this.model.name = data.formName;
            this.model.attributes = this.modelFields;
            this.eventName = "Update";
        }
    }

    onDragStart(event: DragEvent) {
        console.log("drag started", JSON.stringify(event, null, 2));
    }

    onDragEnd(event: DragEvent) {
        console.log("drag ended", JSON.stringify(event, null, 2));
    }

    onDraggableCopied(event: DragEvent) {
        console.log("draggable copied", JSON.stringify(event, null, 2));
    }

    onDraggableLinked(event: DragEvent) {
        console.log("draggable linked", JSON.stringify(event, null, 2));
    }

    onDragged(item: any, list: any[], effect: DropEffect) {
        if (effect === "move") {
            const index = list.indexOf(item);
            list.splice(index, 1);
        }
    }

    onDragCanceled(event: DragEvent) {
        console.log("drag cancelled", JSON.stringify(event, null, 2));
    }

    onDragover(event: DragEvent) {
        console.log("dragover", JSON.stringify(event, null, 2));
    }

    onDrop(event: DndDropEvent, list?: any[]) {
        if (list && (event.dropEffect === "copy" || event.dropEffect === "move")) {

            if (event.dropEffect === "copy") {
                event.data.name = event.data.type + '-' + new Date().getTime();
            }
            let index = event.index;
            if (typeof index === "undefined") {
                index = list.length;
            }
            list.splice(index, 0, event.data);
        }
    }

    addValue(values) {
        values.push(this.value);
        this.value = { label: "", value: "" };
    }

    removeField(i) {
        swal.fire("Are you sure?", "Do you want to remove this field?", 'warning').then((result) => {
            if (result.value) {
                this.model.attributes.splice(i, 1);
            }
        });
    }

    updateForm() {
        console.log(this.model);
        if (this.eventName == "Save") {
            let request = {
                FormName: this.model.name,
                FormJson: JSON.stringify(this.model.attributes)
            }
            this._formBuilderService.addFormBuilder(request).subscribe(result => {
                if (result.state == 0) {
                    this.onSuccess.emit();
                    this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                }
            });
        }
        else {
            let request = {
                RowId: this.model.rowId,
                FormName: this.model.name,
                FormJson: JSON.stringify(this.model.attributes)
            }
            this._formBuilderService.updateFormBuilderbyId(request).subscribe(result => {
                if (result.state == 0) {
                    this.onSuccess.emit();
                    this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                }
            });
        }
    }


    initReport() {
        this.report = true;
        let input = {
            id: this.model._id
        }
        // this.us.getDataApi('/admin/allFilledForms',input).subscribe(r=>{
        //   this.reports = r.data;
        //   console.log('reports',this.reports);
        //   this.reports.map(records=>{
        //     return records.attributes.map(record=>{
        //       if(record.type=='checkbox'){
        //         record.value = record.values.filter(r=>r.selected).map(i=>i.value).join(',');
        //       }
        //     })
        //   });
        // });
    }



    toggleValue(item) {
        item.selected = !item.selected;
    }

    submit() {
        console.log(this.model.attributes);
    }

}
