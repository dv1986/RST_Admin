import { Component, EventEmitter, Output} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GenericValidator } from 'src/app/shared/generic-validator';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MasterService } from '../../master.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
    selector: 'app-email-template-add',
    templateUrl: './email-template-add.component.html'
})
export class EmailTemplateAddComponent{
    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        subject: null,
        htmlString: null
    };
    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: '25rem',
        minHeight: '5rem',
        placeholder: 'Enter text here...',
        translate: 'no',
        uploadUrl: 'v1/images', // if needed
        customClasses: [ // optional
          {
            name: "quote",
            class: "quote",
          },
          {
            name: 'redText',
            class: 'redText'
          },
          {
            name: "titleText",
            class: "titleText",
            tag: "h1",
          },
        ]
      };

    @Output() onSuccess = new EventEmitter<any>();

    // Use with the generic validation message class
    displayMessage: { [key: string]: string } = {};
    private validationMessages: { [key: string]: { [key: string]: string } };
    private genericValidator: GenericValidator;


    constructor(private _masterService: MasterService,
        private notificationService: NotificationService
        ) {
        this.validation();
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Template";
        this.initForm();
    }

    initForm() {
        this.form = new FormGroup({
            'subject': new FormControl('', Validators.required),
            'htmlString': new FormControl('', Validators.required)
        })
    }


    onSubmit() {
        this.model.rowId = 0;
        this.model.subject = this.form.controls['subject'].value;
        this.model.htmlString = this.form.controls['htmlString'].value;
        // this._masterService.addCity(this.model).subscribe(result => {
        //     if (result.state == 0) {
        //         this.onSuccess.emit();
        //         this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
        //     }
        // });
    }


    onCancel(): void {
        this.form.controls['subject'].setValue('');
        this.form.controls['htmlString'].setValue('');
    }

    validation() {
        // Defines all of the validation messages for the form.
        // These could instead be retrieved from a file or database.
        this.validationMessages = {
            subject: {
                required: 'subject name is required.'
            }
        };

        // Define an instance of the validator for use with this form,
        // passing in this form's set of validation messages.
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    // Also validate on blur
    // Helpful if the user tabs through required fields
    blur(): void {
        this.displayMessage = this.genericValidator.processMessages(this.form);
    }
}