import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'pm-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css']
})
export class UserSearchComponent implements OnInit {

  searchForm: FormGroup;
  pageTitle: string;
  constructor() {

  }

  private optionsF1: any;
  searchCriteria = {
    userName: "",
    email: ""
  };

  public parentComponent: any;

  ngOnInit(): void {
    this.pageTitle = "Upload Users";
    this.initForm();
    this.loadSearchCritariaFromLocalStorage();
  }

  initForm() {
    this.searchForm = new FormGroup({
      'userName': new FormControl(),
      'email': new FormControl()
    })
  }


  loadLocalStorage(): void {
    localStorage.setItem(
      "userSearchQuery",
      JSON.stringify(this.searchCriteria)
    );
  }

  loadSearchCritariaFromLocalStorage() {
    if (localStorage.getItem('userSearchQuery')) {
      let userSearchCritaria = JSON.parse(localStorage.getItem("userSearchQuery"));
      this.searchForm.controls['userName'].setValue(userSearchCritaria.userName);
      this.searchForm.controls['email'].setValue(userSearchCritaria.email);
    }
  }

  onSearch() {
    this.searchCriteria.userName = this.searchForm.controls['userName'].value;
    this.searchCriteria.email = this.searchForm.controls['email'].value;
    this.loadLocalStorage();
    this.parentComponent.search();
  }

  onReset() {
    this.searchCriteria.userName = "";
    this.searchCriteria.email = "";
    this.loadLocalStorage();
    this.parentComponent.reset();
  }

  fileToUpload: File = null;
  handleFileInput(files) {
    console.log(files);
    this.fileToUpload = files.item(0);
    this.parentComponent.onUpload(this.fileToUpload);
  }
}
