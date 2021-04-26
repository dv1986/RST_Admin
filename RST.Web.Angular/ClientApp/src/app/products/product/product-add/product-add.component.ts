import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { GenericValidator } from 'src/app/shared/generic-validator';
import { SpinnerService } from 'src/app/shared/services/spinner.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { ProductService } from '../../product.service';
import { SpecificationService } from 'src/app/specification/specification.service';
import { AuthService } from 'src/app/user/auth.service';
import { AttributeService } from 'src/app/attributes/attributes.service';

@Component({
    selector: 'app-product-add',
    templateUrl: './product-add.component.html'
})
export class ProductAddComponent implements OnInit {

    form: FormGroup;
    pageTitle: string;
    model: any = {
        rowId: null,
        categoryParentId: null,
        categoryId: null,
        subCategoryParentId: null,
        subCategoryId: null,
        productTypeId: null,
        productTitle: null,
        shortDescription: null,
        FullDescription: null,
        skuCode: null,
        sellerId: null,
        adminComment: null,
        showOnHomePage: null,
        allowCustomerReviews: null,
        approvedRatingSum: null,
        approvedTotalReviews: null,
        notApprovedTotalReviews: null,
        stockQuantity: null,
        minStockQuantity: null,
        lowStockActivityId: null,
        notifyAdminForQuantityBelow: null,
        orderMinimumQuantity: null,
        orderMaximumQuantity: null,
        allowedQuantities: null,
        displayStockAvailability: null,
        displayStockQuantity: null,
        quantityBeforeUpdate: null,
        isOutOfStock: null,
        disableBuyButton: null,
        disableWishlistButton: null,
        hasDiscountsApplied: null,
        published: null,
        isNew: null,
        displayOrder: null,
        dealProductCategoryId: null,
        commisionRateId: null,
        totalPercentOff: null,
        productFabricId: null,
        seoContentId: null,
        brandId: null,
        productTagId: null,
        productMRP: null,
        retailPrice: null,
        sellingPrice: null,
        specialPrice: null,
        specialPriceStartDate: null,
        specialPriceEndDate: null,
        inclusiveSalesTax: null,
        comments: null,
        attributeIds: null,
        colorIds: null,
        isRecommended: null,
        isFeatured: null,
        createdBy: null,
        modelYear: null,
        modelNumber: null
    };

    parentCategories: any = [];
    categories: any = [];
    subParentCategories: any = [];
    subCategories: any = [];
    productTypes: any = [];
    sizeTypes: any = [];
    brands: any = [];
    tags: any = [];
    fabrics: any = [];
    colors: any = [];
    parentAttributes: any = [];
    attributes: any = [];
    public onAdd: Subject<boolean>;

    @Output() onSuccess = new EventEmitter<any>();

    // Use with the generic validation message class
    displayMessage: { [key: string]: string } = {};
    private validationMessages: { [key: string]: { [key: string]: string } };
    private genericValidator: GenericValidator;

    constructor(private _productService: ProductService,
        private _specificationService: SpecificationService,
        private spinnerService: SpinnerService,
        private _attributeService: AttributeService,
        private notificationService: NotificationService,
        private authService: AuthService) {
        this.validation();
    }

    ngOnInit(): void {
        this.pageTitle = "Add Product";
        this.initForm();
        this.bindParentCategoryDropDown();
        this.bindProdcutSizeType();
        this.bindBrands();
        this.bindTags();
        this.bindFabrics();
        this.bindColors();
        this.onAdd = new Subject();
    }

    bindParentCategoryDropDown() {
        this._productService.getProductCategoryParent("").subscribe(response => {
            if (response.state == 0) {
                this.parentCategories = response.data;
            }
        });
    }

    onCategoryParentChange(params) {
        this._productService.GetCategoryLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.categories = response.data;
            }
        });
    }

    onCategoryChange(params) {
        this._productService.getSubCategoryParentLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.subParentCategories = response.data;
            }
        });
    }

    onSubCategoryParentChange(params) {
        this._productService.getSubCategoryLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.subCategories = response.data;
            }
        });
    }

    onSubCategoryChange(params) {
        this._productService.GetProductTypeLookup(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.productTypes = response.data;
            }
        });
    }

    onProductTypeChange(params) {
        this._attributeService.getProductAttributeParentbyProdcutType(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.parentAttributes = response.data;
            }
        });
    }

    onParentAttributeChange(params) {
        this._attributeService.getproductAttribute(params.rowId).subscribe(response => {
            if (response.state == 0) {
                this.attributes = response.data;
            }
        });
    }

    bindProdcutSizeType() {
        this._specificationService.getProductSizeType("").subscribe(response => {
            if (response.state == 0) {
                this.sizeTypes = response.data;
            }
        });
    }

    bindBrands() {
        this._productService.getBrands("").subscribe(response => {
            if (response.state == 0) {
                this.brands = response.data;
            }
        });
    }

    bindTags() {
        this._specificationService.getProductTag("").subscribe(response => {
            if (response.state == 0) {
                this.tags = response.data;
            }
        });
    }

    bindFabrics() {
        this._specificationService.getProductFabric("").subscribe(response => {
            if (response.state == 0) {
                this.fabrics = response.data;
            }
        });
    }

    bindColors() {
        this._specificationService.getColors("").subscribe(response => {
            if (response.state == 0) {
                this.colors = response.data;
            }
        });
    }




    ImagePath: null;
    urls = new Array<ImageClass>();
    fileToUpload = new Array<File>();
    handleFileInput(files) {
        //this.urls = [];
        if (files) {
            for (let file of files) {
                let reader = new FileReader();
                reader.onload = (e: any) => {
                    this.urls.push({ ImageUrl: e.target.result, imageName: file });
                }
                reader.readAsDataURL(file);
                this.fileToUpload.push(file);
            }
        }
    }


    onDeleteImage(params) {
        debugger;
        console.log(params);
        this.urls = this.urls.filter(a => a != params);
        this.fileToUpload = this.fileToUpload.filter(a => a.name != params.imageName.name);
    }

    initForm() {
        this.form = new FormGroup({
            'categoryParentId': new FormControl('', Validators.required),
            'categoryId': new FormControl(0, Validators.required),
            'subCategoryParentId': new FormControl(null),
            'subCategoryId': new FormControl(null),
            'productTypeId': new FormControl(null),
            'productTitle': new FormControl('', Validators.required),
            'shortDescription': new FormControl('', Validators.required),
            'fullDescription': new FormControl(''),
            'skuCode': new FormControl('', Validators.required),
            'modelYear': new FormControl('', Validators.required),
            'modelNumber': new FormControl('', Validators.required),
            'sellerId': new FormControl(0),
            'adminComment': new FormControl(''),
            // 'approvedRatingSum': new FormControl(''),
            // 'approvedTotalReviews': new FormControl(''),
            // 'notApprovedTotalReviews': new FormControl(''),
            'stockQuantity': new FormControl(0),
            'minStockQuantity': new FormControl(0),
            //'lowStockActivityId': new FormControl(''),
            'notifyAdminForQuantityBelow': new FormControl(0),
            'orderMinimumQuantity': new FormControl(0),
            'orderMaximumQuantity': new FormControl(0),
            'allowedQuantities': new FormControl(''),
            'quantityBeforeUpdate': new FormControl(0),
            //'quantityUpdatedOnUtc': new FormControl('', Validators.required),
            'showOnHomePage': new FormControl(false),
            'allowCustomerReviews': new FormControl(false),
            'displayStockAvailability': new FormControl(false),
            'displayStockQuantity': new FormControl(false),
            'isOutOfStock': new FormControl(false),
            'disableBuyButton': new FormControl(false),
            'disableWishlistButton': new FormControl(false),
            'hasDiscountsApplied': new FormControl(false),
            'published': new FormControl(false),
            'isNew': new FormControl(false),
            'displayOrder': new FormControl(0),
            //'dealProductCategoryId': new FormControl('', Validators.required),
            //'commisionRateId': new FormControl('', Validators.required),
            'totalPercentOff': new FormControl(0),
            'productFabricId': new FormControl(0),
            'seoContentId': new FormControl(0),
            'brandId': new FormControl(null),
            'productTagId': new FormControl(0),
            'productMRP': new FormControl(0),
            'retailPrice': new FormControl(0),
            'sellingPrice': new FormControl(0),
            'specialPrice': new FormControl(0),
            'specialPriceStartDate': new FormControl(null),
            'specialPriceEndDate': new FormControl(null),
            'inclusiveSalesTax': new FormControl(false),
            'comments': new FormControl(''),
            'attributeIds': new FormControl(null),
            'colorIds': new FormControl(null),
            'isRecommended': new FormControl(false),
            'isFeatured': new FormControl(false),
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
        //console.log(this.form.controls['colorIds'].value);
        this.model.rowId = 0;
        this.model.categoryParentId = this.form.controls['categoryParentId'].value;
        this.model.categoryId = this.form.controls['categoryId'].value;
        this.model.subCategoryParentId = this.form.controls['subCategoryParentId'].value;
        this.model.subCategoryId = this.form.controls['subCategoryId'].value;
        this.model.productTypeId = this.form.controls['productTypeId'].value;
        this.model.productTitle = this.form.controls['productTitle'].value;
        this.model.shortDescription = this.form.controls['shortDescription'].value;
        this.model.fullDescription = this.form.controls['fullDescription'].value;
        this.model.skuCode = this.form.controls['skuCode'].value;
        this.model.modelYear = this.form.controls['modelYear'].value;
        this.model.modelNumber = this.form.controls['modelNumber'].value;
        this.model.adminComment = this.form.controls['adminComment'].value;
        this.model.stockQuantity = this.form.controls['stockQuantity'].value;
        this.model.minStockQuantity = this.form.controls['minStockQuantity'].value;
        this.model.notifyAdminForQuantityBelow = this.form.controls['notifyAdminForQuantityBelow'].value;
        this.model.orderMinimumQuantity = this.form.controls['orderMinimumQuantity'].value;
        this.model.orderMaximumQuantity = this.form.controls['orderMaximumQuantity'].value;
        this.model.allowedQuantities = this.form.controls['allowedQuantities'].value;
        this.model.quantityBeforeUpdate = this.form.controls['quantityBeforeUpdate'].value;
        this.model.showOnHomePage = this.form.controls['showOnHomePage'].value;
        this.model.allowCustomerReviews = this.form.controls['allowCustomerReviews'].value;
        this.model.displayStockAvailability = this.form.controls['displayStockAvailability'].value;
        this.model.displayStockQuantity = this.form.controls['displayStockQuantity'].value;
        this.model.isOutOfStock = this.form.controls['isOutOfStock'].value;
        this.model.disableBuyButton = this.form.controls['disableBuyButton'].value;
        this.model.disableWishlistButton = this.form.controls['disableWishlistButton'].value;
        this.model.hasDiscountsApplied = this.form.controls['hasDiscountsApplied'].value;
        this.model.published = this.form.controls['published'].value;
        this.model.isNew = this.form.controls['isNew'].value;
        this.model.displayOrder = this.form.controls['displayOrder'].value;
        this.model.totalPercentOff = this.form.controls['totalPercentOff'].value;
        this.model.productFabricId = this.form.controls['productFabricId'].value;
        this.model.seoContentId = this.form.controls['seoContentId'].value;
        this.model.brandId = this.form.controls['brandId'].value;
        this.model.productTagId = this.form.controls['productTagId'].value;
        this.model.productMRP = this.form.controls['productMRP'].value;
        this.model.retailPrice = this.form.controls['retailPrice'].value;
        this.model.sellingPrice = this.form.controls['sellingPrice'].value;
        this.model.specialPrice = this.form.controls['specialPrice'].value;
        this.model.specialPriceStartDate = this.form.controls['specialPriceStartDate'].value;
        this.model.specialPriceEndDate = this.form.controls['specialPriceEndDate'].value;
        this.model.inclusiveSalesTax = this.form.controls['inclusiveSalesTax'].value;
        this.model.comments = this.form.controls['comments'].value;
        this.model.attributeIds = this.form.controls['attributeIds'].value;
        this.model.colorIds = this.form.controls['colorIds'].value;
        this.model.isRecommended = this.form.controls['isRecommended'].value;
        this.model.isFeatured = this.form.controls['isFeatured'].value;
        this.model.createdBy = Number(this.authService.getLogedUserId());
        console.log(this.model);
        if (!this.Isvalidate())
            return;
        this._productService.addProduct(this.model).subscribe(result => {
            if (result.state == 0) {
                debugger;
                this.onUploadImage(result.data);
                this.onSuccess.emit();
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
            }
        });
    }

    onUploadImage(productId) {
        this._productService.UploadMultipleFiles(this.fileToUpload, productId).subscribe(response => {
            if (response.state == 0) {

            }
        });
    }


    Isvalidate() {
        if (this.model.categoryParentId == undefined || this.model.categoryParentId == "") {
            this.notificationService.ShowError("Please select parent category!", 3000)
            return false
        }
        else if ((this.model.categoryId == undefined || this.model.categoryId == "") && this.categories.length > 0) {
            this.notificationService.ShowError("Please select category!", 3000)
            return false
        }
        else if ((this.model.subCategoryParentId == undefined || this.model.subCategoryParentId == "") && this.subParentCategories.length > 0) {
            this.notificationService.ShowError("Please select parent sub-category!", 3000)
            return false
        }
        else if ((this.model.subCategoryId == undefined || this.model.subCategoryId == "") && this.subCategories.length > 0) {
            this.notificationService.ShowError("Please select sub-category!", 3000)
            return false
        }
        else if ((this.model.productTypeId == undefined || this.model.productTypeId == "") && this.productTypes.length > 0) {
            this.notificationService.ShowError("Please select product type!", 3000)
            return false
        }
        else if ((this.model.attributeIds == undefined || this.model.attributeIds == "") && this.parentAttributes.length > 0) {
            this.notificationService.ShowError("Please select attributes!", 3000)
            return false
        }

        if (this.model.stockQuantity < 0) {
            this.notificationService.ShowError("Stock Quantity should not be less than zero!", 3000)
            return false
        }
        else if (this.model.minStockQuantity < 0) {
            this.notificationService.ShowError("Min Stock Quantity should not be less than zero!", 3000)
            return false
        }
        else if (this.model.notifyAdminForQuantityBelow < 0) {
            this.notificationService.ShowError("Notify Admin for quantity below should not be less than zero!", 3000)
            return false
        }
        else if (this.model.orderMinimumQuantity < 0) {
            this.notificationService.ShowError("Minimum Order Qty should not be less than zero!", 3000)
            return false
        }
        else if (this.model.orderMaximumQuantity < 0) {
            this.notificationService.ShowError("Maximum Order Qty should not be less than zero!", 3000)
            return false
        }
        else if (this.model.totalPercentOff < 0) {
            this.notificationService.ShowError("Total Percent Off should not be less than zero!", 3000)
            return false
        }
        else if (this.model.productMRP < 0) {
            this.notificationService.ShowError("MRP should not be less than zero!", 3000)
            return false
        }
        else if (this.model.retailPrice < 0) {
            this.notificationService.ShowError("Retail Price should not be less than zero!", 3000)
            return false
        }
        else if (this.model.sellingPrice < 0) {
            this.notificationService.ShowError("Selling Price should not be less than zero!", 3000)
            return false
        }
        else if (this.model.specialPrice < 0) {
            this.notificationService.ShowError("Special Price should not be less than zero!", 3000)
            return false
        }

        return true;
    }


    onCancel(): void {
        this.form.reset();
    }

    validation() {
        // Defines all of the validation messages for the form.
        // These could instead be retrieved from a file or database.
        this.validationMessages = {
            productTitle: {
                required: 'product Title is required.'
            },
            skuCode: {
                required: 'sku Code is required.'
            },
            shortDescription: {
                required: 'short description is required.'
            }
        };

        // Define an instance of the validator for use with this form,
        // passing in this form's set of validation messages.
        this.genericValidator = new GenericValidator(this.validationMessages);
    }

    // Also validate on blur
    // Helpful if the user tabs through required fields
    blur(): void {
        this.displayMessage = this.genericValidator.processMessages(this.form);
    }
}

export class ImageClass {
    imageName: File;
    ImageUrl: string;
}