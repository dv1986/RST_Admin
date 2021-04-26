// import { Component } from "@angular/core";
// import { DomSanitizer } from '@angular/platform-browser';

// @Component({
//   selector: 'app-image-formatter-cell',
//   // template: `<img   width= '75px'
//   // height= '75px' border="0" onError="this.outerHTML='No Image'"  src=\"{{ params.value }}\">`
//   // template: `<img   width= '35px'
//   // height= '25px' border="0" [src]="getImage()">`
//   template: `<img   width= '35px'
//   height= '25px' onError="this.outerHTML='No Image'" border="0" [src]="getImage()">`
// })

// export class ImageViewer {
//   params: any;
//   image: any;

//   constructor(private sanitizer: DomSanitizer) {

//   }

//   agInit(params: any) {
//     this.params = params.value;
//     this.getImage();
//   }

//   getImage() {
//     //return this.sanitizer.bypassSecurityTrustResourceUrl(this.params);
//     // console.log(this.params)
//     return this.params;
//   }

// }

import { Component } from "@angular/core";
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-image-formatter-cell',
  template: `<img   width= '35px'
  height= '25px' border="0" onError="this.outerHTML='No Image'"  src=\"{{ params }}\">`
})

export class ImageViewer {
  params: any;
  agInit(params: any) {
    this.params = environment.ImageBaseUrl + params.value + "?" + new Date().getTime();
  }
}