import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { BuildFormComponent } from './build-form/build-form.component';
import { DndModule } from 'ngx-drag-drop';
// import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { FormCreatorComponent } from './form-creator/form-creator.component';
import { FormsDemoComponent } from './forms-demo/forms-demo.component';
import { FormListComponent } from './form-list/form-list.component';
import { FormBuilderShellComponent } from './form-builder-shell.component';

const userRoutes: Routes = [
  { path: 'Build-Form', component: BuildFormComponent },
  { path: 'Forms-Demo', component: FormsDemoComponent },
  { path: 'Forms-List', component: FormBuilderShellComponent },
];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(userRoutes),
    TabsModule,
    NgSelectModule,
    AngularEditorModule,
    // SweetAlert2Module.forRoot(),
    DndModule
  ],
  declarations: [
      BuildFormComponent,
      FormCreatorComponent,
      FormsDemoComponent,
      FormListComponent,
      FormBuilderShellComponent
  ],
  entryComponents: [],
})
export class FormBuilderModule { }
