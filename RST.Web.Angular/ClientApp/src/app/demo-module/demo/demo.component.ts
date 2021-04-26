import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { GridOptions } from 'ag-grid-community';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { GridFilterItem } from 'src/app/shared/components/grid/grid-filters/GridFilterItem';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { getdatePickerCellEditor, getNumericCellEditor } from 'src/app/shared/components/grid/components';
import { UserSearchComponent } from 'src/app/user/user-search/user-search.component';
import { UserService } from 'src/app/user/user.service';


@Component({
  selector: 'app-demo-list',
  templateUrl: './demo.component.html'
})
export class DemoComponent implements OnInit {


  users: any[] = [];
  public columnDefs: any[];
  public gridOptions: GridOptions = <GridOptions>{};
  public gridInformation: GridInformation = { id: "", updatedData: [], selectedData: [] };
  public gridActions: any[];
  filterComponent: GridFilterItem = new GridFilterItem(UserSearchComponent, this);
  @ViewChild('addUserRefrenceModal') addUserRefrenceModal: TemplateRef<any>;

  searchCriteria = {
    userName: "",
    email: ""
  };

  constructor(private _userService: UserService,
    private modalService: BsModalService
  ) {
    this.initGrid();
    this.initGridActions();
  }

  ngOnInit(): void {
    this.loadData();
    this.resetLocalStorage();
  }

  loadData() {
    this._userService.getDemo(this.searchCriteria.userName).subscribe(response => {
      if (response.state == 0)
        if (response.data != null && response.data.length > 0) {
          this.users = response.data;
        }
        else {
          this.users = [];
        }
    });
  }

  private modalRef: BsModalRef;
  onAddUser() {
    //this.openModal(this.addUserRefrenceModal);
    // var initialState = {
    //   isModal: true,
    //   isEditMode: false
    // };
    // this.modalRef = this.modalService.show(RegistrationComponent, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" })
    // if (this.modalRef) {
    //   this.modalRef.content.onAdd.subscribe(result => {
    //     console.log('results', result);
    //   })
    // }
  }

  openModal(template: TemplateRef<any>) {
    const config: ModalOptions = {
      backdrop: 'static',
      keyboard: false,
      animated: true,
      ignoreBackdropClick: true,
      class: 'modal-lg'
    };
    this.modalRef = this.modalService.show(template, config);
  }

  onUpdate() {
    console.log(this.gridInformation);
  }

  onSelectionChanged(params) {
    console.log(params);
  }



  onRefresh() {
    this.loadSearchCritariaFromLocalStorage();
    this.loadData();
    this.gridInformation.selectedData = [];
    this.gridInformation.updatedData = [];
    this.gridOptions.api.deselectAll();
  }

  search() {
    if (this.gridOptions.context.thisComponent.showFilter == true)
      this.gridOptions.context.thisComponent.toggleFilter();
    this.onRefresh();
  }

  reset() {
    if (this.gridOptions.context.thisComponent.showFilter == true)
      this.gridOptions.context.thisComponent.toggleFilter();

    this.resetLocalStorage();
    this.onRefresh();
  }

  resetLocalStorage() {
    localStorage.removeItem("userSearchQuery");
    this.searchCriteria.userName = "";
    this.searchCriteria.email = "";
  }

  loadSearchCritariaFromLocalStorage() {
    if (localStorage.getItem('userSearchQuery')) {
      let userSearchCritaria = JSON.parse(localStorage.getItem("userSearchQuery"));
      this.searchCriteria.userName = userSearchCritaria.userName;
      this.searchCriteria.email = userSearchCritaria.email;
    }
    console.log(this.searchCriteria);
  }

  manageTabs(param) {

  }

  //#region Required function to initialize grid and actions
  onGridReady() {
    this.gridOptions.context["mainComponent"] = this;
  }

  initGridActions() {
    this.gridActions = [
      {
        actionId: 1,
        title: "Refresh",
        cssClass: "btn btn-warning btn-xs",
        onClick: function (event, context) {
          context.mainComponent.onRefresh();
        },
        requiresConfirmation: false,
        isDisabled: false
      },
      {
        actionId: 2,
        title: "Add User",
        cssClass: "btn btn-primary btn-xs",
        onClick: function (event, context) {
          context.mainComponent.onAddUser();
        },
        requiresConfirmation: false,
        isDisabled: false
      },
      {
        actionId: 3,
        title: "Update",
        cssClass: "btn btn-success btn-xs",
        onClick: function (event, context) {
          context.mainComponent.onUpdate();
        },
        requiresConfirmation: false,
        isDisabled: false

      },
      {
        actionId: 4,
        title: "Delete",
        cssClass: "btn btn-danger btn-xs",
        onClick: function (event, context) {

        },
        requiresConfirmation: true,
        confirmationPlacement: "bottom",/*bottom,left,top,right*/
        confirmationTitle: "Confirm",
        confirmationMessage: "Do you want to continue delete the selected record?",
        confirmationYesAction: function (event, context) {
          context.mainComponent.onUpdate();
        },
        isDisabled: false
      }
    ];
  }

  initGrid() {
    this.gridOptions.columnDefs = [
      { headerName: 'ID', field: 'rowId', hide: true },
      {
        headerName: 'Name', field: 'name', sortable: true, filter: true, type: ['editableColumn']
        , cellClassRules: {
          'norm-spec-Q-red-highlight': function (params) {
            try {
              return (params.data.name == "Deepak")
            } catch (e) {
              console.log(params.data)
            }
          },
        },
      },
      { headerName: 'Email Address', field: 'email', filter: true, type: ["Email"] },
      {
        headerName: 'Date of Birth', field: 'dateField', filter: true,
        type: ['Date', 'editableColumn'], cellEditor: "datePickerCellEditor", editable: true
      },
      {
        headerName: 'Drop Down Field', field: 'status'
        , cellEditor: 'agSelectCellEditor'
        , cellEditorParams: this.fillcombostatus
        , valueGetter: this.lookUpValueGetter
        , type: ['editableColumn']
      },
      { headerName: 'Phone', field: 'phone', filter: true, type: ['editableColumn', 'Phone'], cellEditor: "numericCellEditor" },
      { headerName: 'Currency', field: 'currencyField', type: ["Currency"], filter: true, aggFunc: "sum" },
      {
        headerName: 'Percentage Number', field: 'percentage', type: ["Percentage Number"], filter: true, aggFunc: "sum"
      },
      { headerName: 'Percentage Decimal', field: 'percentageDecimal', type: ["Percentage Decimal"], filter: true, aggFunc: "sum" },
      { headerName: 'Decimal Column', field: 'decimal2Digit', type: ["Decimal"], filter: true, aggFunc: "sum" },
      { headerName: 'Decimal 4 Column', field: 'decimal4Digit', type: ["Decimal4"], filter: true, aggFunc: "sum" },
      {
        headerName: 'Decimal Value Formatter', field: 'decimal4Digit',
        valueFormatter: params => Number(params.data.ratio).toFixed(4),
        filter: true, aggFunc: "sum"
      },
      { headerName: 'Number Column', field: 'numberField', type: ["numberColumn"], filter: true, aggFunc: "sum" },
      { headerName: 'message', field: '' },
    ];
    this.gridOptions.defaultColDef = { resizable: true };
    this.gridOptions.components = {
      datePickerCellEditor: getdatePickerCellEditor(),
      numericCellEditor: getNumericCellEditor()
    };
  }
  //#endregion

  //type:["Date & Time", "Time", "Date"]
  //type:["Email", "Phone", "Currency"]
  //type:["numberColumn", "Decimal", "Decimal4"]
  //type:["Percentage Number", "Percentage Decimal"]


  lookUpValueGetter(params) {
    if (params.data !== undefined)
      return params.data.status;
  }

  fillcombostatus(param) {
    return {
      values: ["N-New Year Created", "P-Quoting In Progres", "Q-Quote With Customer",
        "C-Current/Live", "L-Lost", "A-Won/Archived"]
    }
  }
}
