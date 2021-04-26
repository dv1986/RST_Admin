import { Component, OnInit, Input, forwardRef, OnChanges, SimpleChanges, EventEmitter, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


@Component({
  selector: 'app-cos-utc-date-picker',
  templateUrl: './cos-utc-date-picker.component.html',
  styleUrls: ['./cos-utc-date-picker.component.css'],
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => CosUtcDatePickerComponent), multi: true }]
})
export class CosUtcDatePickerComponent implements OnInit, ControlValueAccessor, OnChanges {

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.SelectedDate) {
      console.log('Hi Date is changing');

    }

    }
    @Output() onChange = new EventEmitter<any>();
  @Input() minDate: Date = null;
  @Input() maxDate: Date = null;
  @Input('SelectedDate') _selectedDate: Date;

  @Input() disabled: boolean = false;
  get SelectedDate() {
    return this._selectedDate;
    //return new Date(this._selectedDate.getTime() - this._selectedDate.getTimezoneOffset() * 60 * 1000);
  }

  set SelectedDate(val) {
    if (!this.isValidDate(val)) {
        if (val) {
            let date = new Date(val.toString());
            this._selectedDate = date;
            this.propagateChange(new Date(this._selectedDate.getTime() - this._selectedDate.getTimezoneOffset() * 60 * 1000));
            this.onChange.emit(this._selectedDate);
        }
        else {
            this._selectedDate = val;
            this.propagateChange(this._selectedDate);
            this.onChange.emit(this._selectedDate);
        }
    }
    else {
      this._selectedDate = val;
        this.propagateChange(new Date(this._selectedDate.getTime() - this._selectedDate.getTimezoneOffset() * 60 * 1000));
        this.onChange.emit(this._selectedDate);
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
       // if (obj!=null && obj!="")
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
    return d instanceof Date;
  }

}
