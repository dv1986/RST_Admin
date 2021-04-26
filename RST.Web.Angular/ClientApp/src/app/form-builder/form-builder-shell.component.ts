import { Component, ViewChild } from '@angular/core';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  templateUrl: './form-builder-shell.component.html'
})
export class FormBuilderShellComponent {

  public tabsList = {
    tabFormList: true,
    tabFormAdd: false,
    tabFormDemo: false
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "FL":
        this.tabset.tabs[0].active = true;
        this.tabsList.tabFormList = true;
        break;
      case "FA":
        this.tabset.tabs[1].active = true;
        this.tabsList.tabFormAdd = true;
        break;
      case "FD":
        this.tabset.tabs[2].active = true;
        this.tabsList.tabFormDemo = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabFormList = false;
    this.tabsList.tabFormAdd = false;
    this.tabsList.tabFormDemo = false;
  }
}
