import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from './user/auth-guard.service';

import { ShellComponent } from './home/shell.component';
import { PageNotFoundComponent } from './home/page-not-found.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './home/dashboard.component';

const appRoutes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'Product-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./products/product.module').then(m => m.ProductModule)
      },
      {
        path: 'Master',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./master/master.module').then(m => m.MasterModule)
      },
      {
        path: 'Demo-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./demo-module/demo.module').then(m => m.DemoModule)
      },
      {
        path: 'Attribute-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./attributes/attributes.module').then(m => m.AttributeModule)
      },
      {
        path: 'Notification-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./notifications/notification.module').then(m => m.NotificationModule)
      },
      {
        path: 'Specification-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./specification/specification.module').then(m => m.SpecificationModule)
      },
      {
        path: 'FormBuilder-Module',
        //canActivate: [AuthGuard],
        loadChildren: () =>
          import('./form-builder/form-builder.module').then(m => m.FormBuilderModule)
      },
      {
        path: '',
        loadChildren: () =>
          import('./user/user.module').then(m => m.UserModule)
      }
    ]
  },
  { 
    path: 'login', 
    component: LoginComponent,
    children:[
      {
        path: '',
        loadChildren: () =>
          import('./login/login.module').then(m => m.LoginModule)
      }
    ] 
  },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
