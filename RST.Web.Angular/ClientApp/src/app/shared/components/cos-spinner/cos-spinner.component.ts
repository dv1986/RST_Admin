import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SpinnerService } from '../../services/spinner.service';

@Component({
  selector: 'app-cos-spinner',
  templateUrl: './cos-spinner.component.html',
  styleUrls: ['./cos-spinner.component.css']
})
export class CosSpinnerComponent implements OnInit, OnDestroy {
  ngOnInit(): void {
   
  }

  
  _template: string = `
  <div style="color: #64d6e2" class="la-ball-clip-rotate-multiple la-3x">
    <div></div>
    <div></div>
    <div></div>
  </div>`;

  
  _loadingText: string = 'Loading .......';
   
  _zIndex: number = 999;
  
  @Input()
  public set zIndex(value: number) {
    this._zIndex = value;
  }

 
  public get zIndex(): number {
    return this._zIndex;
  }

  
  @Input()
  public set template(value: string) {
    this._template = value;
  }


  
  public get template(): string {
    return this._template;
  }


  
  @Input()
  public set loadingText(value: string) {
    this._loadingText = value;
  }


  
  public get loadingText(): string {
    return this._loadingText;
  }

  _threshold: number = 500;
  
  @Input()
  public set threshold(value: number) {
    this._threshold = value;
  }


  
  public get threshold(): number {
    return this._threshold;
  }

  
  subscription: Subscription;

  
  showSpinner = false;


  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  constructor(private spinnerService: SpinnerService ) {
    let timer: any;

    this.subscription =
      this.spinnerService.getMessage().subscribe(data => {
        if (data.show) {
          if(data.message !="")
          {this._loadingText = data.message;}
          timer = setTimeout(function () {
            this.showSpinner = data.show;
          }.bind(this), this.threshold);
        } else {
          clearTimeout(timer);
          this.showSpinner = false;
        }
      });
  }
   

}
