import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CodeGeneratorComponent } from './code-generator/code-generator.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { DemoComponent } from './demo/demo.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NgSelectModule } from '@ng-select/ng-select';

const userRoutes: Routes = [
    { path: 'Code-Generator', component: CodeGeneratorComponent },
    { path: 'Grid-Demo', component: DemoComponent },
  ];
  
@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(userRoutes),
    TabsModule,
    //CollapseModule,
    ButtonsModule,
    NgSelectModule
    ],
    declarations: [CodeGeneratorComponent,DemoComponent],
    entryComponents: []
})
export class DemoModule { }
