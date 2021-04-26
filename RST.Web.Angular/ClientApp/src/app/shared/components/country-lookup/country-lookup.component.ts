import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { MasterService } from 'src/app/master/master.service';
import { distinctUntilChanged, debounceTime, switchMap } from 'rxjs/operators'

@Component({
    selector: 'app-country-lookup',
    templateUrl: './country-lookup.component.html',
    providers: [
        { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => CountryLookupComponent), multi: true }
    ]
})
export class CountryLookupComponent implements OnInit, ControlValueAccessor {

    countries: any = [];
    @Input() disabled: boolean = false;
    @Input() placeholder: string = "Select country";
    @Input('selectedCountry') _selectedCountry: any;
    get selectedCountry() {
        return this._selectedCountry;
    }

    set selectedCountry(val) {
        this._selectedCountry = val;
        this.propagateChange(val);
        this.countryChange.emit(val);
    }


    @Output() countryChange = new EventEmitter<any>();

    propagateChange = (_: any) => { };
    validateFn: any = () => { };
    onTouched: any = () => { /*Empty*/ }

    writeValue(obj: any): void {
        //if (obj != null && obj!="")
        this._selectedCountry = obj;
    }
    registerOnChange(fn: any): void {
        this.propagateChange = fn;
    }
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }
    setDisabledState?(isDisabled: boolean): void {
        //throw new Error("Method not implemented.");
    }

    typeahead = new EventEmitter<string>();
    constructor(private lookupService: MasterService) { }

    // ngOnInit() {

    //     this.typeahead.pipe(distinctUntilChanged(),
    //         debounceTime(300)
    //         , switchMap(term => this.lookupService.getCountryLookup((term == null) ? "" : term)))
    //         .subscribe(result => {
    //             if (result.state == "0") {
    //                 this.countries = result.data;
    //             }
    //         });
    // }

    ngOnInit() {
        this.lookupService.getCountry("").subscribe(response =>{
          if(response.state == 0)
          {
            this.countries = response.data;
          }
        });
      }

}
