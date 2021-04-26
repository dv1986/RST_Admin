import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';

@Component({
    selector: 'app-category-add',
    templateUrl: './category-add.component.html'
})
export class CategoryAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        productCategoryParentId: null,
        catName: null,
        description: null,
        seoContentId: null,
        productImageId: null,
        displayOrder: null,
        includeInTopMenu: null,
        isNew: null,
        hasDiscountApplied: null,
        isPublished: null,
    };

    parentCategories: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();


    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private spinnerService: SpinnerService,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Category";
        this.initForm();
        this.bindParentCategoryDropDown();
        this.onAdd = new Subject();
    }

    bindParentCategoryDropDown() {
        this._productService.getProductCategoryParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentCategories = response.data;
            }
        });
    }

    bindSeoDropDown() {
        this._productService.getProductCategoryParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentCategories = response.data;
            }
        });
    }

    initForm() {
        this.form = new FormGroup({
            'productCategoryParentId': new FormControl('', Validators.required),
            'catName': new FormControl('', Validators.required),
            'description': new FormControl(''),
            'seoContentId': new FormControl(0),
            'displayOrder': new FormControl(0),
            'includeInTopMenu': new FormControl(false),
            'isNew': new FormControl(false),
            'hasDiscountApplied': new FormControl(false),
            'isPublished': new FormControl(false),
        })
    }


   // convenience getter for easy access to form fields
   get f() { return this.form.controls; }
   submitted = false;
   onSubmit() {

       this.submitted = true;
       // stop here if form is invalid
       if (this.form.invalid) {
           return;
       }
        this.model.rowId = 0;
        this.model.productCategoryParentId = this.form.controls['productCategoryParentId'].value;
        this.model.catName = this.form.controls['catName'].value;
        this.model.description = this.form.controls['description'].value;
        this.model.seoContentId = this.form.controls['seoContentId'].value;
        this.model.displayOrder = this.form.controls['displayOrder'].value.toString();
        this.model.includeInTopMenu = this.form.controls['includeInTopMenu'].value;
        this.model.isNew = this.form.controls['isNew'].value;
        this.model.hasDiscountApplied = this.form.controls['hasDiscountApplied'].value;
        this.model.isPublished = this.form.controls['isPublished'].value;
        if (this.model.productImageId == null)
            this.model.productImageId = 0;
        this._productService.addProductCategory(this.model).subscribe(result => {
            if (result.state == 0) {
                this.onAdd.next(true);
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }

    fileToUpload: File = null;
    handleFileInput(files) {
        console.log(files);
        this.fileToUpload = files.item(0);
        this._productService.upload(this.fileToUpload).subscribe(result => {
            if (result.state == 0) {
                this.notificationService.ShowSuccess("Image uploaded!");
                this.model.productImageId = result.data.rowId;
            }
        });
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
