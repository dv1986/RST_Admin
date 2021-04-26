import { Component, OnInit, Input, forwardRef, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


@Component({
  selector: 'app-cos-date-picker',
  templateUrl: './cos-date-picker.component.html',
  styleUrls: ['./cos-date-picker.component.css'],
  providers:[{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => CosDatePickerComponent), multi: true }]
})
export class CosDatePickerComponent implements OnInit, ControlValueAccessor, OnChanges {
 
  ngOnChanges(changes: SimpleChanges): void {
   if(changes.SelectedDate)
   {
     console.log('Hi Date is changing');
     
   }
   
    }
    @Output() onChange = new EventEmitter<any>();
  @Input() minDate: Date = null;
  @Input() maxDate: Date = null;
  @Input('SelectedDate') _selectedDate: Date;

  @Input() disabled :boolean = false;
  get SelectedDate() {
    return this._selectedDate;
  }

  set SelectedDate(val) {

    if(!this.isValidDate(val))
    {
        if (val) {
            let date = new Date(val.toString());
            this._selectedDate = date;
            this.propagateChange(val);
            this.onChange.emit(val);
        }
        else {
            this._selectedDate = val;
            this.propagateChange(val);
            this.onChange.emit(val);
        }
    }
    else
    {
      this._selectedDate = val;
        this.propagateChange(val);
        this.onChange.emit(val);
    }
   
   
  }

  propagateChange = (_: any) => { };
  onTouched: any = () => { /*Empty*/ }

  constructor() {
    if (this.minDate == null) {
      this.minDate = new Date();
      this.minDate.setFullYear(this.minDate.getFullYear() - 10);
    }
    if (this.maxDate == null) {
      this.maxDate = new Date();
    }
  }

  ngOnInit() {

  }

    writeValue(obj: any): void {
        
      //if (obj != null && obj!="")
      this.SelectedDate = obj;
  }
  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
   // throw new Error("Method not implemented.");
  }

  isValidDate(d) {
    return d instanceof Date ;
  }

}
