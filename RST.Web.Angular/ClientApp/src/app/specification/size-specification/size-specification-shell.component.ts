import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './size-specification-shell.component.html'
})
export class SizeSpecificationShellComponent {

  public tabsList = {
    tabMeasureDimensionList: true,
    tabSizeTypeList: false
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "MDL":
        this.tabsList.tabMeasureDimensionList = true;
        break;
      case "STL":
        this.tabsList.tabSizeTypeList = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabMeasureDimensionList = false;
    this.tabsList.tabSizeTypeList = false;
  }
}
