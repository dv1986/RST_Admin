import { Component, OnInit, forwardRef, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { MasterService } from 'src/app/master/master.service';
import { distinctUntilChanged, debounceTime, switchMap } from 'rxjs/operators'

@Component({
    selector: 'app-state-lookup',
    templateUrl: './state-lookup.component.html',
    providers: [
        { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => StateLookupComponent), multi: true }
    ]
})
export class StateLookupComponent implements OnInit, ControlValueAccessor, OnChanges {

    states: any = [];
    @Input() countryId = 0;

    @Input() disabled: boolean = false;
    @Input() placeholder: string = "Select state";
    @Input('selectedState') _selectedState: any;
    get selectedState() {
        return this._selectedState;
    }

    set selectedState(val) {
        this._selectedState = val;
        this.propagateChange(val);
        this.stateChange.emit(val);
    }


    @Output() stateChange = new EventEmitter<any>();

    propagateChange = (_: any) => { };
    validateFn: any = () => { };
    onTouched: any = () => { /*Empty*/ }

    writeValue(obj: any): void {
        //if (obj != null && obj!="")
        this._selectedState = obj;
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
    //         , switchMap(term => this.lookupService.getStateLookup((term == null) ? "" : term)))
    //         .subscribe(result => {
    //             if (result.state == "0") {
    //                 this.states = result.data;
    //             }
    //         });
    // }

    ngOnInit() {
        this.lookupService.getState("").subscribe(response => {
            if (response.state == 0) {
                this.states = response.data;
            }
        });
    }

    ngOnChanges(changes: SimpleChanges): void {

        if (changes.countryId && !changes.countryId.isFirstChange()) {
            this.lookupService.getStateLookup(this.countryId).subscribe(response => {
                if (response.state == 0) {
                    this.states = response.data;
                }
            });
        }
    }

}
