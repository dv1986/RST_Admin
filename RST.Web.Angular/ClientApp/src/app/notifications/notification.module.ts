import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { SharedModule } from '../shared/shared.module';
import { NotificationListComponent } from './notification-list/notification-list.component';


const notificationRoutes: Routes = [
    { path: 'Notification', component: NotificationListComponent },
  ];
  
  @NgModule({
    imports: [
      SharedModule,
      RouterModule.forChild(notificationRoutes),
      TabsModule,
      NgSelectModule,
    ],
    declarations: [
        NotificationListComponent
    ],
    entryComponents: [
    ]
  })
  export class NotificationModule { }