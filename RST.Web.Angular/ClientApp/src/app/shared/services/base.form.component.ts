import { AfterViewChecked, ViewChild, Injector } from "@angular/core";
import { NgForm } from "@angular/forms";
import { NotificationService } from "./notification.service";

export class BaseForm implements AfterViewChecked
{
    protected notification: NotificationService;
    constructor(injector: Injector){
        this.notification = injector.get(NotificationService);
    }
    @ViewChild('dataForm') currentForm: NgForm;
    protected dataForm: NgForm;
    ngAfterViewChecked(): void {
        this.formChange();
    }

    formChange() {
        if (this.currentForm === this.dataForm) { return; }
      
        this.dataForm = this.currentForm;
        if (this.dataForm) {
          this.dataForm.valueChanges
              .subscribe(data => this.onValueChanged());
        }
      }

      onValueChanged() {
        let messages:string[] =[];
        if (!this.dataForm) { return; }
        const form = this.dataForm.form;        
          for(const field in form.controls)
          {
             // console.log(field);
              const control = form.get(field);
             
              if (control && control.dirty && !control.valid) {
               // console.log(control.errors);
               // messages.push(`${field} ${control.errors[0].AppValidationMessages}`)
              }        
          }
        this.notification.alertErrors(messages);
        
          /*for (const field in this.formErrors) {
          this.formErrors[field] = '';
          const control = form.get(field);
      
          if (control && control.dirty && !control.valid) {
            const messages = AppValidationMessages.errorMessages[field];
            for (const key in control.errors) {
              this.formErrors[field] = messages[key];
            }
          }
        }*/
      }

}