import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './product-category-shell.component.html'
})
export class ProductCategoryShellComponent {

  public tabsList = {
    tabParentCategoryList: true,
    tabCategoryList: false,
    tabParentSubCategoryList: false,
    tabSubCategoryList: false,
    tabBrandsList: false,
    tabProductType: false,
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "PC":
        this.tabsList.tabParentCategoryList = true;
        break;
      case "CL":
        this.tabsList.tabCategoryList = true;
        break;
      case "PSCL":
        this.tabsList.tabParentSubCategoryList = true;
        break;
      case "SCL":
        this.tabsList.tabSubCategoryList = true;
        break;
      case "BL":
        this.tabsList.tabBrandsList = true;
        break;
      case "TL":
        this.tabsList.tabProductType = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabParentCategoryList = false;
    this.tabsList.tabCategoryList = false;
    this.tabsList.tabParentSubCategoryList = false;
    this.tabsList.tabSubCategoryList = false;
    this.tabsList.tabBrandsList = false;
    this.tabsList.tabProductType = false;
  }
}
