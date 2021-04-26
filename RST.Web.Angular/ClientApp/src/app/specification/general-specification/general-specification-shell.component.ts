import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './general-specification-shell.component.html'
})
export class GeneralSpecificationShellComponent {

  public tabsList = {
    tabColorList: true,
    tabFabricList: false,
    tabTagList: false,
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "CL":
        this.tabsList.tabColorList = true;
        break;
      case "FL":
        this.tabsList.tabFabricList = true;
        break;
        case "TL":
          this.tabsList.tabTagList = true;
          break;
    }
  }

  resetTabs() {
    this.tabsList.tabColorList = false;
    this.tabsList.tabFabricList = false;
    this.tabsList.tabTagList = false;
  }
}
