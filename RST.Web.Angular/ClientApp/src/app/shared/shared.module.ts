import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { AlertModule } from 'ngx-bootstrap/alert';
import { TooltipModule } from 'ngx-bootstrap/tooltip'; 
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationPopoverModule } from 'angular-confirmation-popover';
import { CosUtcDatePickerComponent } from './components/cos-utc-date-picker/cos-utc-date-picker.component';
import { CosSpinnerComponent } from './components/cos-spinner/cos-spinner.component';
import { CosDatePickerComponent } from './components/cos-date-picker/cos-date-picker.component';
import { CosAlertComponent } from './components/cos-alert/cos-alert.component';
import { ConfirmDialogueComponent } from './components/confirm-dialogue/confirm-dialogue.component';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AgGridModule } from 'ag-grid-angular';
import { AgGridComponent } from './components/grid/ag-grid.comonent';
import { WinModuleLaunchButtonComponent } from './components/win-module-launch-button-component/win-module-launch-button.component';
import { GridFiltersComponent } from './components/grid/grid-filters/grid-filters.component';
import { GridFilterDirective } from './components/grid/grid-filters/grid-filter.directive';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NotificationService } from './services/notification.service';
import { SpinnerService } from './services/spinner.service';
import { CountryLookupComponent } from './components/country-lookup/country-lookup.component';
import { StateLookupComponent } from './components/state-lookup/state-lookup.component';
import { SeoLookupComponent } from './components/seo-lookup/seo-lookup.component';
import { FileUploadComponent } from './components/file-upload/file-upload.component';
import { Imagecompressionpopup } from './components/image-compression-popup/image-compression-popup.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgSelectModule,
    BsDatepickerModule,
    AlertModule,
    TooltipModule,
    BsDropdownModule,
    AgGridModule.withComponents([]),
    ModalModule.forRoot(),
    ConfirmationPopoverModule.forRoot({
      confirmButtonType: 'danger' // set defaults here,

    }),
  ],
  declarations: [
    CosAlertComponent,
    CosDatePickerComponent,
    CosSpinnerComponent,
    CosUtcDatePickerComponent,
    ConfirmDialogueComponent,
    AgGridComponent,
    WinModuleLaunchButtonComponent,
    GridFiltersComponent,
    GridFilterDirective,
    CountryLookupComponent,
    StateLookupComponent,
    SeoLookupComponent,
    FileUploadComponent,
    Imagecompressionpopup
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CosAlertComponent,
    CosDatePickerComponent,
    CosSpinnerComponent,
    CosUtcDatePickerComponent,
    ConfirmDialogueComponent,
    AgGridModule,
    AgGridComponent,
    WinModuleLaunchButtonComponent,
    CountryLookupComponent,
    StateLookupComponent,
    SeoLookupComponent,
    FileUploadComponent
  ],
  entryComponents: [ConfirmDialogueComponent, Imagecompressionpopup],
  providers:[
    BsModalService
    , ToastrService 
    // , NotificationService
    // ,SpinnerService
  ]
})
export class SharedModule { }
