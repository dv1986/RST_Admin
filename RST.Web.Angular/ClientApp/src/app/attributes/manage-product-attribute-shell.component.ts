import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './manage-product-attribute-shell.component.html'
})
export class ManageProductAttributeShellComponent {

  public tabsList = {
    tabProductAttributeParentList: false,
    tabProductAttributeList: false,
    tabCategoryAttributeMappingList: true
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "CAML":
        this.tabsList.tabCategoryAttributeMappingList = true;
        break;
      case "PAPL":
        this.tabsList.tabProductAttributeParentList = true;
        break;
      case "PAL":
        this.tabsList.tabProductAttributeList = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabProductAttributeParentList = false;
    this.tabsList.tabProductAttributeList = false;
    this.tabsList.tabCategoryAttributeMappingList = false;
  }
}
