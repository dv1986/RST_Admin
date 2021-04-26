import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MasterService } from '../../master.service';

@Component({
  selector: 'app-city-search',
  templateUrl: './city-search.component.html'
})
export class CitySearchComponent implements OnInit {

  searchForm: FormGroup;
  pageTitle: string;
  constructor(private lookupService: MasterService,) {

  }

  private optionsF1: any;
  searchCriteria = {
    countryId: 0,
    stateId: 0
  };
  countries: any = [];
  states: any = [];

  public parentComponent: any;

  ngOnInit(): void {
    this.pageTitle = "Search";
    this.initForm();
    this.bindCountryDropDown();
    this.loadSearchCritariaFromLocalStorage();
  }

  initForm() {
    this.searchForm = new FormGroup({
      'countryId': new FormControl(),
      'stateId': new FormControl()
    })
  }

  bindCountryDropDown() {
    this.lookupService.getCountry("").subscribe(response => {
      if (response.state == 0) {
        this.countries = response.data;
      }
    });
  }

  bindStateDropDown(params) {
    if (params == null)
      return;
    this.lookupService.getStateLookup(params).subscribe(response => {
      if (response.state == 0) {
        this.states = response.data;
      }
    });
  }

  onCountryChange(params) {
    this.bindStateDropDown(params.rowId);
  }


  loadLocalStorage(): void {
    localStorage.setItem(
      "citySearchQuery",
      JSON.stringify(this.searchCriteria)
    );
  }

  loadSearchCritariaFromLocalStorage() {
    if (localStorage.getItem('citySearchQuery')) {
      let searchRequest = JSON.parse(localStorage.getItem("citySearchQuery"));
      this.searchForm.controls['countryId'].setValue(searchRequest.countryId);
      this.searchForm.controls['stateId'].setValue(searchRequest.stateId);
      this.bindStateDropDown(searchRequest.countryId);
    }
  }

  onSearch() {
    this.searchCriteria.countryId = this.searchForm.controls['countryId'].value;
    this.searchCriteria.stateId = this.searchForm.controls['stateId'].value;
    this.loadLocalStorage();
    this.parentComponent.search();
  }

  onReset() {
    this.searchCriteria.countryId = 0;
    this.searchCriteria.stateId = 0;
    this.loadLocalStorage();
    this.parentComponent.reset();
  }
}
