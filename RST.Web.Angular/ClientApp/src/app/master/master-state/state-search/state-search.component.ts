import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-state-search',
  templateUrl: './state-search.component.html'
})
export class StateSearchComponent implements OnInit {

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
      "stateSearchQuery",
      JSON.stringify(this.searchCriteria)
    );
  }

  loadSearchCritariaFromLocalStorage() {
    if (localStorage.getItem('stateSearchQuery')) {
      let searchRequest = JSON.parse(localStorage.getItem("stateSearchQuery"));
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
