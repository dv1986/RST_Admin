import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SharedModule } from '../shared/shared.module';

import { RegistrationComponent } from './registration/registration.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserSearchComponent } from './user-search/user-search.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ButtonsModule} from 'ngx-bootstrap/buttons';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { TreeviewModule } from 'ngx-treeview'
import { UserPermissionComponent } from './user-permission/user-permission.component';
import { NgSelectModule } from '@ng-select/ng-select';

const userRoutes: Routes = [
  // { path: 'login', component: LoginComponent },
  { path: 'User-Module/Users', component: UserListComponent },
  { path: 'forgotpassword', component: ForgotPasswordComponent },
  
];
 
@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(userRoutes),
    TabsModule,
    //CollapseModule,
    ButtonsModule,
    TreeviewModule,
    NgSelectModule
  ],
  declarations: [
    UserListComponent,
    UserSearchComponent,
    RegistrationComponent,
    ForgotPasswordComponent,
    UserPermissionComponent,
    
  ],
  entryComponents: [UserSearchComponent],
})
export class UserModule { }
