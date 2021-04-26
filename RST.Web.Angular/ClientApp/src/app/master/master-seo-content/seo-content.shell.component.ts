import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';


@Component({
  selector: 'app-seo-content-shell',
  templateUrl: './seo-content.shell.component.html'
})
export class SeoContentShellComponent {
  public tabsList = {
    tabRecordList: true,
    tabAddNew: false
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "L":
        this.tabset.tabs[0].active = true;
        this.tabsList.tabRecordList = true;
        break;
      case "A":
        this.tabset.tabs[1].active = true;
        this.tabsList.tabAddNew = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabRecordList = false;
    this.tabsList.tabAddNew = false;
  }
}