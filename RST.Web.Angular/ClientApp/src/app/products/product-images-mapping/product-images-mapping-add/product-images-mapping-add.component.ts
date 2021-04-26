import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Subject } from 'rxjs';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductService } from '../../product.service';

@Component({
    selector: 'app-product-images-mapping-add',
    templateUrl: './product-images-mapping-add.component.html'
})
export class ProductImageMappingAddComponent implements OnInit { 

    pageTitle: string;
    productId = null;
    parentCategories: any = [];
    public onAdd: Subject<boolean>;
    @Output() onSuccess = new EventEmitter<any>();

    constructor(private _productService: ProductService,
        public bsModalRef: BsModalRef,
        private notificationService: NotificationService) {
    }

    ngOnInit(): void {
        this.pageTitle = "Add New Image";
        this.onAdd = new Subject();
    }


    onSubmit() {
        this._productService.UploadMultipleFiles(this.fileToUpload, this.productId).subscribe(response => {
            if (response.state == 0) {
                this.onAdd.next(true);
                this.notificationService.ShowSuccess("Record added sucessfully!", 3000);
                this.bsModalRef.hide();
            }
        });
    }

    fileToUpload = new Array<File>();
    handleFileInput(files) {
        //this.urls = [];
        if (files) {
            for (let file of files) {
                this.fileToUpload.push(file);
            }
        }
    }


    onCancel(): void {
        this.bsModalRef.hide();
    }
}
