import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { LayoutComponent } from './layout.component';
import { LayoutRoutingModule } from './layout-routing.module';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LayoutServiceService } from './layout-service.service';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  imports: [
    CommonModule,
    LayoutRoutingModule,
    BsDropdownModule,
    CollapseModule,
    SharedModule
    ],
    providers: [LayoutServiceService],
  declarations: [HeaderComponent, SidebarComponent, LayoutComponent, DashboardComponent]
})
export class LayoutModule { }
