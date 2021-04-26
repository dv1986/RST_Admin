import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login.component';
import { StartupComponent } from './startup/startup.component';


const routes: Routes = [
  {
    path: '', component: StartupComponent, children: [
      { path: '', component: LoginComponent }
    ]
  }

]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  declarations: []
})
export class LoginRoutingModule { }
