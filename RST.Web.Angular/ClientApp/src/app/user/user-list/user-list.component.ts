import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { UserService } from '../user.service';
import { GridOptions } from 'ag-grid-community';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { GridInformation } from 'src/app/shared/components/grid/grid-information';
import { GridFilterItem } from 'src/app/shared/components/grid/grid-filters/GridFilterItem';
import { UserSearchComponent } from '../user-search/user-search.component';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { getdatePickerCellEditor, getNumericCellEditor } from 'src/app/shared/components/grid/components';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { MasterService } from 'src/app/master/master.service';
import { Imagecompressionpopup } from 'src/app/shared/components/image-compression-popup/image-compression-popup.component';
import * as XLSX from 'xlsx';


@Component({
  selector: 'pm-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {


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

  public tabsList = {
    tabUserList: true,
    tabAddUser: false,
    tabUserPermission: false
  };

  @ViewChild('tabset') tabset: TabsetComponent;

  constructor(private _userService: UserService,
    private modalService: BsModalService,
    public _masterService: MasterService,
    private notificationService: NotificationService,
    // private spinnerService: SpinnerService
  ) {
    this.initGrid();
    this.initGridActions();
    this.fillcomboUserType();
    this.fillcomboSubscription();
  }

  ngOnInit(): void {
    this.loadData();
    this.resetLocalStorage();
  }

  manageTabs(params) {
    this.resetTabs();
    switch (params) {
      case "UL":
        this.tabset.tabs[0].active = true;
        this.tabsList.tabUserList = true;
        this.loadData();
        break;
      case "AU":
        this.tabset.tabs[1].active = true;
        this.tabsList.tabAddUser = true;
        break;
      case "UP":
        this.tabset.tabs[2].active = true;
        this.tabsList.tabUserPermission = true;
        break;
    }
  }

  resetTabs() {
    this.tabsList.tabUserList = false;
    this.tabsList.tabAddUser = false;
    this.tabsList.tabUserPermission = false;
  }

  loadData() {
    this._userService.getAllUsers(this.searchCriteria.userName).subscribe(response => {
      if (response.state == 0)
        if (response.data != null && response.data.length > 0) {
          this.users = response.data;
        }
        else {
          this.users = [];
        }
    });
  }

  onAddUser() {
    this.manageTabs("AU");
  }

  onUpdate() {
    console.log(this.gridInformation.updatedData);
    if (this.gridInformation.updatedData.length == 0) {
      this.notificationService.ShowError("Please select record(s) to update data!");
      return;
    }
    this._userService.updateUser(this.gridInformation.updatedData).subscribe(response => {
      if (response.state == 0) {
        this.onRefresh();
        this.notificationService.ShowSuccess("Record updated sucessfully!", 3000);
      }
      else if (response.state == 4) {
        for (let rowindex in response.data) {
          let item = this.users.find(fn => fn.rowId == response.data[rowindex].rowId)
          item.message = response.data[rowindex].message;
          if (item.message != "")
            this.notificationService.ShowWarning("Please check Message Column", 3000)
        }
        this.gridOptions.api.refreshCells({ force: true });
      }
    });
  }

  onDelete() {
    if (this.gridInformation.selectedData.length == 0) {
      this.notificationService.ShowError("Please select record(s) to update data!");
      return;
    }
    this._userService.deleteUser(this.gridInformation.selectedData).subscribe(response => {
      if (response.state == 0) {
        this.notificationService.ShowSuccess("Record deleted sucessfully!", 3000);
        this.onRefresh();
      }
      else if (response.state == 4) {
        for (let rowindex in response.data) {
          let item = this.users.find(fn => fn.rowId == response.data[rowindex].rowId)
          item.message = response.data[rowindex].message;
          if (item.message != "")
            this.notificationService.ShowWarning("Please check Message Column", 3000)
        }
        this.gridOptions.api.refreshCells({ force: true });
      }
    });
  }


  private modalRef: BsModalRef;
  onUpdateAdvertisement() {
    if (this.gridInformation.selectedData[0].userTypeName != "Advertiser") {
      this.notificationService.ShowError("Invalid user for advertisement.")
    }
    else {
      const initialState = {
        ImagePath: this.gridInformation.selectedData[0].imagePath,
        ImageName: this.gridInformation.selectedData[0].imageName,
        moduleName: "advertisement",
        rowId: this.gridInformation.selectedData[0].rowId
      };

      this.modalRef = this.modalService.show(Imagecompressionpopup, { initialState: initialState, backdrop: 'static', keyboard: false, animated: true, ignoreBackdropClick: true, class: "modal-lg" });

      if (this.modalRef) {
        this.modalRef.content.onAdd.subscribe(result => {
          this.onRefresh();
        })
      }
    }
  }



  onSelectionChanged(params) {
    if (this.gridInformation.selectedData.length > 0)
      localStorage.setItem(
        "selecteduserId",
        JSON.stringify(this.gridInformation.selectedData[0].rowId)
      );
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

  private file: File;
  private arrayBuffer: any;
  private datalist: any = [];
  public datalistupload: any = [];
  onUpload(file) {
    if (this.gridOptions.context.thisComponent.showFilter == true)
      this.gridOptions.context.thisComponent.toggleFilter();
    //this.loadSearchQuery();
    this.file = file;
    this.Upload();
  }

  Upload() {
    debugger;
    let fileReader = new FileReader();

    fileReader.onload = (e) => {
      this.arrayBuffer = fileReader.result;
      var data = new Uint8Array(this.arrayBuffer);
      var arr = new Array();
      for (var i = 0; i != data.length; ++i) arr[i] = String.fromCharCode(data[i]);
      var bstr = arr.join("");
      var workbook = XLSX.read(bstr, { type: "binary" });
      var first_sheet_name = workbook.SheetNames[0];
      var worksheet = workbook.Sheets[first_sheet_name];

      debugger;
      this.datalist = XLSX.utils.sheet_to_json(worksheet, { raw: false });
      this.datalist.forEach(element => {
        
      });
      this._userService.addBulkUser(this.datalist).subscribe(result => {
        if (result.state == 0) {
          this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
          this.reset();
        }
      })
    }
    fileReader.readAsArrayBuffer(this.file);
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
          context.mainComponent.onDelete();
        },
        isDisabled: false
      },
      {
        actionId: 5,
        title: "Update Advertisement",
        cssClass: "btn btn-success btn-xs",
        onClick: function (event, context) {
          context.mainComponent.onUpdateAdvertisement();
        },
        requiresConfirmation: false,
        isDisabled: false

      },
    ];
  }

  initGrid() {
    this.gridOptions.columnDefs = [
      { headerName: '', field: 'rowId', hide: true },
      { headerName: '', field: 'userTypeId', hide: true },
      { headerName: '', field: 'subscriptionId', hide: true },
      { headerName: '', field: 'advertiseImageId', hide: true },
      { headerName: '', field: 'imagePath', hide: true },
      { headerName: '', field: 'imageName', hide: true },
      { headerName: '', field: 'thumbnailPath', hide: true },
      {
        headerName: 'First Name', field: 'firstname', sortable: true, filter: true, type: ['editableColumn']
        , cellClassRules: {
          'norm-spec-Q-red-highlight': function (params) {
            try {
              return (params.data.userTypeName == "Admin")
            } catch (e) {
              console.log(params.data)
            }
          },
        },
      },
      {
        headerName: 'Last Name', field: 'lastname', sortable: true, filter: true, type: ['editableColumn']
      },
      {
        headerName: 'User Type', field: 'userTypeName', sortable: true, filter: true, type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
        cellEditorParams: { values: this.userTypeList }, valueGetter: this.lookUpValueGetterforUserType,
        valueSetter: this.lookUpValueSetterforUserType
      },
      {
        headerName: 'Subscription Type', field: 'subscriptionName', sortable: true, filter: true, type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
        cellEditorParams: { values: this.subscriptionTypeList }, valueGetter: this.lookUpValueGetterforSubscriptionType,
        valueSetter: this.lookUpValueSetterforSubscriptionType
      },
      // { headerName: 'Email', field: 'email', filter: true, type: ['Email', 'editableColumn'] },
      { headerName: 'Email', field: 'email', filter: true, type: ['editableColumn'] },
      {
        headerName: 'Gender', field: 'gender', type: ['editableColumn'], cellEditor: 'agSelectCellEditor',
        cellEditorParams: this.fillGenderCombostatus, valueGetter: this.lookUpValueGetterforGender
      },
      {
        headerName: 'Date of Birth', field: 'dateOfBirth', filter: true,
        type: ['Date', 'editableColumn'], cellEditor: "datePickerCellEditor"
      },
      { headerName: 'Mobile Number', field: 'mobile', filter: true, type: ['Phone', 'editableColumn'] },
      { headerName: 'Phone Number', field: 'phone', filter: true, type: ['editableColumn', 'Phone'], cellEditor: "numericCellEditor" },
      { headerName: 'Garage Name', field: 'garageName', sortable: true, filter: true, type: ['editableColumn'] },
      { headerName: 'Address Line1', field: 'addressLine1', type: ['editableColumn'] },
      { headerName: 'Address Line2', field: 'addressLine2', type: ['editableColumn'] },
      { headerName: 'Country', field: 'countryName', sortable: true, filter: true },
      { headerName: 'State', field: 'stateName', sortable: true, filter: true },
      { headerName: 'City', field: 'cityName', sortable: true, filter: true },
      { headerName: 'Zip Code', field: 'zipCode', type: ['editableColumn'], cellEditor: "numericCellEditor" },
      { headerName: 'Password', field: 'password', sortable: true, filter: true, type: ['editableColumn'] },
      { headerName: 'message', field: 'message', type: ["Error"] }
    ];
    this.gridOptions.defaultColDef = { resizable: true };
    this.gridOptions.components = {
      datePickerCellEditor: getdatePickerCellEditor(),
      numericCellEditor: getNumericCellEditor()
    };
  }
  //#endregion

  fillGenderCombostatus(param) {
    return {
      values: ['M', 'F']
    }
  }
  lookUpValueGetterforGender(params) {
    if (params.data !== undefined)
      return params.data.gender;
  }

  userTypeList: any[] = [];
  fillcomboUserType() {
    this._userService.getUserType().subscribe(response => {
      if (response.state == 0) {
        response.data.forEach(element => {
          this.userTypeList.push(`${element.rowId}-${element.userTypeName}`);
        });
      }
    });
  }

  lookUpValueGetterforUserType(params) {
    if (params.data !== undefined) {
      return params.data.userTypeName;
    }
  }

  lookUpValueSetterforUserType(params) {
    if (params.data !== undefined) {
      var id = params.newValue.split('-')[0].trim();
      params.data.userTypeId = Number(id);

      var text = params.newValue.split('-')[1].trim();
      params.data.userTypeName = text;
      return params.data.userTypeName;
    }
  }


  subscriptionTypeList: any[] = [];
  fillcomboSubscription() {
    this._masterService.getSubscriptionList().subscribe(response => {
      if (response.state == 0) {
        response.data.forEach(element => {
          this.subscriptionTypeList.push(`${element.rowId}-${element.subscriptionName}`);
        });
      }
    });
  }

  lookUpValueGetterforSubscriptionType(params) {
    if (params.data !== undefined) {
      return params.data.subscriptionName;
    }
  }

  lookUpValueSetterforSubscriptionType(params) {
    if (params.data !== undefined) {
      var id = params.newValue.split('-')[0].trim();
      params.data.subscriptionId = Number(id);

      var text = params.newValue.split('-')[1].trim();
      params.data.subscriptionName = text;
      return params.data.subscriptionName;
    }
  }

}
