import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

// Imports for loading & configuring the in-memory web api

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { ShellComponent } from './home/shell.component';
import { MenuComponent } from './home/menu.component';
import { PageNotFoundComponent } from './home/page-not-found.component';
import { AgGridModule } from 'ag-grid-angular';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { AlertModule } from 'ngx-bootstrap/alert';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { TreeviewModule } from 'ngx-treeview'
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { SharedModule } from './shared/shared.module';
import { AuthInterceptorService } from './shared/guard/auth-interceptor.service';
import { ErrorInterceptorService } from './shared/guard/error-interceptor.service';
import { DashboardComponent } from './home/dashboard.component';
import { NotificationService } from './shared/services/notification.service';
import { SpinnerService } from './shared/services/spinner.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ImageViewer } from './shared/components/ImageViewer/ImageViewer';
// import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { DndModule } from 'ngx-drag-drop';


/* Feature Modules */

@NgModule({
  imports: [
    SharedModule,
    BrowserModule,
    HttpClientModule,
    AgGridModule.withComponents([ImageViewer]),
    AppRoutingModule,
    TooltipModule.forRoot(),
    // AlertModule.forRoot(),
    // BsDropdownModule.forRoot(),
    // CollapseModule.forRoot(),
    // BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    TreeviewModule.forRoot(),
    //SweetAlert2Module.forRoot(),
    //DndModule
  ],
  declarations: [
    AppComponent,
    ShellComponent,
    MenuComponent,
    DashboardComponent,
    PageNotFoundComponent,
    ImageViewer
  ],
  bootstrap: [AppComponent],
  providers: [
    BsModalService
    , ToastrService
    , NotificationService
    , SpinnerService
    , {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptorService,
      multi: true
    }
    , {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
  ]
})
export class AppModule { }
