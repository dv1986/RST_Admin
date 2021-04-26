import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-win-module-launch-button',
  templateUrl: './win-module-launch-button.component.html',
  styleUrls: ['./win-module-launch-button.component.css'],
})
export class WinModuleLaunchButtonComponent implements OnInit {

  constructor(private sanitizer: DomSanitizer) { }
  @Input('css-class') cssClass: string = '';
  @Input() params: any;
  @Input() title: string;
  @Input('module-name') moduleName: string= '';

  url: any;
  ngOnInit() {
    var link = environment.LaunchProtocol + "://" + this.moduleName  + "/"+ this.params;

    // for (var key in this.params) {
    //   if (this.params.hasOwnProperty(key)) {
    //     link += '&' + key + '=' + this.params[key];
    //   }
    // }

    this.url = this.sanitizer.bypassSecurityTrustUrl(link);
  }
}
