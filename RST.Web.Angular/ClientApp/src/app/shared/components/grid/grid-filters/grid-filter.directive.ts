import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appFilter]'
})
export class GridFilterDirective {

    constructor(public viewContainerRef: ViewContainerRef) { }

}
