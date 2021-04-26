import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ProductService } from 'src/app/products/product.service';
import { environment } from 'src/environments/environment';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-image-compression-popup',
  templateUrl: './image-compression-popup.component.html',
  styleUrls: ['./image-compression-popup.component.css']
})
export class Imagecompressionpopup implements OnInit {

  ImageName: any = {};
  ImagePath: any = {};
  moduleName: any;
  rowId: any;
  isDisable = true;
  isViewModeOnly = false;
  public onAdd: Subject<boolean>;
  constructor(public bsModalRef: BsModalRef,
    private _productService: ProductService,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.onAdd = new Subject();
    if (this.moduleName == "product")
      this.isViewModeOnly = true
    this.ImagePath = environment.ImageBaseUrl + this.ImagePath + "?" + new Date().getTime();
  }

  fileToUpload: File = null;
  handleFileInput(files) {
    this.isDisable = false;
    this.fileToUpload = files.item(0);

    if (files && files.item(0)) {
      const reader = new FileReader();
      reader.onload = e => this.ImagePath = reader.result;
      reader.readAsDataURL(this.fileToUpload);
    }
  }

  onCancel() {
    this.bsModalRef.hide();
  }
  onUpdate() {
    this._productService.UpdateImage(this.fileToUpload, this.ImageName, this.moduleName, this.rowId).subscribe(result => {
      if (result.state == 0) {
        this.notificationService.ShowSuccess("Image uploaded successfully!", 3000);
        this.onAdd.next(true);
        this.bsModalRef.hide();
      }
    });
  }
}


