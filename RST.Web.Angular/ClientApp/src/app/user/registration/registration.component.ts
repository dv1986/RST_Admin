import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from '../user.service';
import { Subject } from 'rxjs';
import { MasterService } from 'src/app/master/master.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { ProductService } from 'src/app/products/product.service';
import { SpinnerService } from 'src/app/shared/services/spinner.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  userForm: FormGroup;
  pageTitle: string;
  user: any = {
    firstName: null,
    lastname: null,
    dateOfBirth: null,
    email: null,
    gender: null,
    mobile: null,
    phone: null,
    garageName: null,
    rowId: null,
    searchAddress: null,
    addressline1: null,
    addressline2: null,
    zipcode: null,
    countryId: null,
    stateId: null,
    cityId: null,
    password: null,
    userTypeId: null,
    subscriptionId: null,
    advertiseImageId: null
  };
  errorMessage = '';
  emailPattern = "^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$";

  public onAdd: Subject<boolean>;

  countries: any = [];
  states: any = [];
  cities: any = [];
  userTypes: any[];
  subscriptionList: any[];
  genders: any = [{ id: "M", text: "M" }, { id: "F", text: "F" }];
  @Output() onSuccess = new EventEmitter<any>();

  constructor(private _userService: UserService
    , public _masterService: MasterService
    , private notificationService: NotificationService,
    private _productService: ProductService
    , private _spinnerService: SpinnerService
  ) {
  }

  ngOnInit(): void {
    this.pageTitle = "Add New User";
    this.initForm();
    this.onAdd = new Subject();
    this.bindCountryDropDown();
    this.bindUserTypeDropDown();
    this.bindSubscriptionDropDown();
  }

  // bindGenderDropDown() {
  //   this._masterService.getCountry("").subscribe(response => {
  //     if (response.state == 0) {
  //       this.countries = response.data;
  //     }
  //   });
  // }

  bindCountryDropDown() {
    this._masterService.getCountry("").subscribe(response => {
      if (response.state == 0) {
        this.countries = response.data;
      }
    });
  }

  onCountryChange(params) {
    this._masterService.getStateLookup(params.rowId).subscribe(response => {
      if (response.state == 0) {
        this.states = response.data;
      }
    });
  }
  onStateChange(params) {
    this._masterService.getCityLookup(params.rowId).subscribe(response => {
      if (response.state == 0) {
        this.cities = response.data;
      }
    });
  }

  bindUserTypeDropDown() {
    this._userService.getUserType().subscribe(response => {
      if (response.state == 0) {
        this.userTypes = response.data;
      }
    });
  }

  bindSubscriptionDropDown() {
    this._masterService.getSubscriptionList().subscribe(response => {
      if (response.state == 0) {
        this.subscriptionList = response.data;
      }
    });
  }

  isAdvertiser = false;
  onUserTypeChange(params) {
    if (params.userTypeName == 'Advertiser') {
      this.isAdvertiser = true;

      // this.userForm.controls['gender'] = new FormControl('');
      // this.userForm.controls['garageName'] = new FormControl('');
      // this.userForm.controls['password'] = new FormControl('');
      // this.userForm.controls['subscriptionId'] = new FormControl('');
      // this.userForm.controls['confirmPassword'] = new FormControl('');
    }
    else {
      // this.userForm.controls['gender'] = new FormControl('', Validators.required);
      // this.userForm.controls['garageName'] = new FormControl('', Validators.required);
      // this.userForm.controls['password'] = new FormControl('', [Validators.required, Validators.minLength(6)]);
      // this.userForm.controls['subscriptionId'] = new FormControl('', Validators.required);
      // this.userForm.controls['confirmPassword'] = new FormControl('', [Validators.required, Validators.minLength(6)]);
    }
  }


  initForm() {
    this.userForm = new FormGroup({
      'firstName': new FormControl('', Validators.required),
      'lastname': new FormControl('', Validators.required),
      'email': new FormControl('', [Validators.required, Validators.pattern("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$")]),
      // 'email': new FormControl(''),
      // 'gender': new FormControl('', Validators.required),
      'gender': new FormControl('M'),
      'mobile': new FormControl('1234567890', [Validators.required, Validators.pattern('^[0-9]*$')]),
      'phone': new FormControl('', [Validators.pattern('^[0-9]*$')]),
      'garageName': new FormControl('', Validators.required),
      // 'dateOfBirth': new FormControl('', Validators.required),
      'dateOfBirth': new FormControl(null),
      'searchAddress': new FormControl(''),
      // 'addressline1': new FormControl('', Validators.required),
      'addressline1': new FormControl(''),
      'addressline2': new FormControl(''),
      // 'zipcode': new FormControl('', [Validators.required]),
      'zipcode': new FormControl(''),
      // 'password': new FormControl('', [Validators.required, Validators.minLength(6)]),
      'password': new FormControl(''),
      // 'countryId': new FormControl('', Validators.required),
      // 'stateId': new FormControl('', Validators.required),
      // 'cityId': new FormControl('', Validators.required),
      'countryId': new FormControl(null),
      'stateId': new FormControl(null),
      'cityId': new FormControl(null),
      'userTypeId': new FormControl(3, Validators.required),
      'subscriptionId': new FormControl(1, Validators.required),
      // 'confirmPassword': new FormControl('', [Validators.required, Validators.minLength(6)])
      'confirmPassword': new FormControl('')
    })
  }


  // convenience getter for easy access to form fields
  get f() { return this.userForm.controls; }
  submitted = false;
  onSubmit() {

    this.submitted = true;
    // stop here if form is invalid
    if (this.userForm.invalid) {
      return;
    }
    this.user.rowId = 0;
    this.user.firstName = this.userForm.controls['firstName'].value;
    this.user.lastname = this.userForm.controls['lastname'].value;
    this.user.email = this.userForm.controls['email'].value;
    this.user.mobile = this.userForm.controls['mobile'].value;
    this.user.gender = this.userForm.controls['gender'].value;
    this.user.dateOfBirth = this.userForm.controls['dateOfBirth'].value;
    this.user.phone = this.userForm.controls['phone'].value;
    this.user.garageName = this.userForm.controls['garageName'].value;
    this.user.addressline1 = this.userForm.controls['addressline1'].value;
    this.user.addressline2 = this.userForm.controls['addressline2'].value;
    this.user.zipcode = this.userForm.controls['zipcode'].value;
    this.user.countryId = this.userForm.controls['countryId'].value;
    this.user.stateId = this.userForm.controls['stateId'].value;
    this.user.cityId = this.userForm.controls['cityId'].value;
    this.user.password = this.userForm.controls['password'].value;
    this.user.subscriptionId = this.userForm.controls['subscriptionId'].value;
    this.user.userTypeId = this.userForm.controls['userTypeId'].value;
    //this.user.searchAddress = this.userForm.controls['searchAddress'].value;
    if (!this.Isvalidate())
      return;

    this._userService.addUser(this.user).subscribe(result => {
      if (result.state == 0) {
        this.onSuccess.emit();
        this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
      }
    })
  }

  Isvalidate() {

    if (this.userForm.controls['confirmPassword'].value != this.user.password) {
      this.notificationService.ShowError("password mismatched!", 3000);
      return false;
    }
    return true;
  }

  fileToUpload: File = null;
  handleFileInput(files) {
    this._spinnerService.show();
    console.log(files);
    this.fileToUpload = files.item(0);
    this._productService.upload(this.fileToUpload).subscribe(result => {
      this._spinnerService.hide();
      if (result.state == 0) {
        this.notificationService.ShowSuccess("Image uploaded!");
        this.user.advertiseImageId = result.data.rowId;
      }
    });
  }


  onCancel(): void {
    this.userForm.reset()
    //this.bsModalRef.hide();
  }
}
