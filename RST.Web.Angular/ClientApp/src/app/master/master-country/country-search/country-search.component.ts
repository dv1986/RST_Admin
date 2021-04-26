import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-country-search',
  templateUrl: './country-search.comonent.html'
})
export class CountrySearchComponent implements OnInit {

  searchForm: FormGroup;
  pageTitle: string;
  constructor() {

  }

  private optionsF1: any;
  searchCriteria = {
    countryName: ""
  };

  public parentComponent: any;

  ngOnInit(): void {
    this.pageTitle = "Search";
    this.initForm();
    this.loadSearchCritariaFromLocalStorage();
  }

  initForm() {
    this.searchForm = new FormGroup({
      'countryName': new FormControl()
    })
  }


  loadLocalStorage(): void {
    localStorage.setItem(
      "countrySearchQuery",
      JSON.stringify(this.searchCriteria)
    );
  }

  loadSearchCritariaFromLocalStorage() {
    if (localStorage.getItem('countrySearchQuery')) {
      let searchRequest = JSON.parse(localStorage.getItem("countrySearchQuery"));
      this.searchForm.controls['countryName'].setValue(searchRequest.countryName);
    }
  }

  onSearch() {
    this.searchCriteria.countryName = this.searchForm.controls['countryName'].value;
    this.loadLocalStorage();
    this.parentComponent.search();
  }

  onReset() {
    this.searchCriteria.countryName = "";
    this.loadLocalStorage();
    this.parentComponent.reset();
  }
}
