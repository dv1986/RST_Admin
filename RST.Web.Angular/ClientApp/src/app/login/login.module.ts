import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { LoginRoutingModule } from './login-routing.module';
import { FormsModule } from '@angular/forms';
import {AlertModule} from 'ngx-bootstrap/alert';
import { StartupComponent } from './startup/startup.component'
import { SharedModule } from '../shared/shared.module';


@NgModule({
  imports: [
    CommonModule, LoginRoutingModule, FormsModule,AlertModule ,SharedModule
  ],
  declarations: [LoginComponent, StartupComponent]
})
export class LoginModule { 
  

}
