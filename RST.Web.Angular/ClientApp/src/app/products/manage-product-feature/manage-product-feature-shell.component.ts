import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './manage-product-feature-shell.component.html'
})
export class ManageProductFeatureShellComponent {

  public tabsList = {
    tabProductFeatureCategoryList: true,
    tabProductFeatureList: false,
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "PFCL":
        this.tabsList.tabProductFeatureCategoryList = true;
        break;
      case "PFL":
        this.tabsList.tabProductFeatureList = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabProductFeatureCategoryList = false;
    this.tabsList.tabProductFeatureList = false;
  }
}
