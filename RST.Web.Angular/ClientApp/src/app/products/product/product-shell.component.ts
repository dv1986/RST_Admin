import { Component, ViewChild } from '@angular/core';
import { angularEditorConfig } from '@kolkov/angular-editor/lib/config';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './product-shell.component.html'
})
export class ProductShellComponent {

  // public bottomTabsList = {
  //   tabAttributes: true,
  //   tabReviews: false,
  // };

  public tabsList = {
    tabProductList: true,
    tabAddNew: false,
  };

  @ViewChild('tabset') tabset: TabsetComponent;
  @ViewChild('bottomtabset') bottomtabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "PL":
        this.tabset.tabs[0].active = true;
        this.tabsList.tabProductList = true;
        break;
      case "PA":
        //alert(params)
        this.tabset.tabs[1].active = true;
        this.tabsList.tabAddNew = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabProductList = false;
    this.tabsList.tabAddNew = false;
  }

  // DisplayBottomTabs(params) {
  //   switch (params) {
  //     case "AL":
  //       this.bottomtabset.tabs[0].active = true;
  //       this.bottomTabsList.tabAttributes = true;
  //       break;
  //     case "RL":
  //       this.bottomtabset.tabs[1].active = true;
  //       this.bottomTabsList.tabReviews = true;
  //       break;
  //   }
  // }
}
