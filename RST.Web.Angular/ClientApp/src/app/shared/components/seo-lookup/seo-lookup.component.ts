import { Component, OnInit, forwardRef, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { MasterService } from 'src/app/master/master.service';
import { distinctUntilChanged, debounceTime, switchMap } from 'rxjs/operators'

@Component({
    selector: 'app-seo-lookup',
    templateUrl: './seo-lookup.component.html',
    providers: [
        { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => SeoLookupComponent), multi: true }
    ]
})
export class SeoLookupComponent implements OnInit, ControlValueAccessor {

    seoList: any = [];
    @Input() disabled: boolean = false;
    @Input() placeholder: string = "Select seo";
    @Input('selectedSeo') _selectedSeo: any;
    get selectedSeo() {
        return this._selectedSeo;
    }

    set selectedSeo(val) {
        this._selectedSeo = val;
        this.propagateChange(val);
        this.seoChange.emit(val);
    }


    @Output() seoChange = new EventEmitter<any>();

    propagateChange = (_: any) => { };
    validateFn: any = () => { };
    onTouched: any = () => { /*Empty*/ }

    writeValue(obj: any): void {
        //if (obj != null && obj!="")
        this._selectedSeo = obj;
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
    //         , switchMap(term => this.lookupService.getseoLookup((term == null) ? "" : term)))
    //         .subscribe(result => {
    //             if (result.state == "0") {
    //                 this.countries = result.data;
    //             }
    //         });
    // }

    ngOnInit() {
        this.lookupService.getSeoContent("").subscribe(response =>{
          if(response.state == 0)
          {
            this.seoList = response.data;
          }
        });
      }

}
