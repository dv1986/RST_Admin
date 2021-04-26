import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'form-creator',
    templateUrl: './form-creator.component.html'
})
export class FormCreatorComponent implements OnInit {

    @Input() options: any;
    public fieldsApi: any;

    ngOnInit() {
        if (this.options.context != null) {
            this.options.context.thisComponent = this;
        }
        else {
            this.options.context = {
                thisComponent: this
            }
        }

        this.options.fieldsApi = {
            parent: this,
            GetFieldById(id) {
                var result = this.parent.options.attributes.find(x => x.formControlName == id);
                if (result != undefined && result != null)
                    return result;
                return null;
            }
        }
    }

    valueChanged(value, obj, data = null) {
        if (obj.onChange != null)
            obj.onChange({ field: obj, value: value, data: data, context: this.options.context });
    }
}