import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { SharedModule } from '../shared/shared.module';
import { GeneralSpecificationShellComponent } from './general-specification/general-specification-shell.component';
import { ColorAddComponent } from './general-specification/manage-color/color-add/color-add.component';
import { ColorListComponent } from './general-specification/manage-color/color-list/color-list.component';
import { FabricAddComponent } from './general-specification/manage-fabric/fabric-add/fabric-add.component';
import { ProductFabricListComponent } from './general-specification/manage-fabric/fabric-list/fabric-list.component';
import { TagAddComponent } from './general-specification/manage-tag/tag-add/tag-add.component';
import { TagListComponent } from './general-specification/manage-tag/tag-list/tag-list.component';
import { MeasureDimensionAddComponent } from './size-specification/measure-dimension/measure-dimension-add/measure-dimension-add.component';
import { MeasureDimensionListComponent } from './size-specification/measure-dimension/measure-dimension-list/measure-dimension-list.component';
import { SizeSpecificationShellComponent } from './size-specification/size-specification-shell.component';
import { SizeTypeAddComponent } from './size-specification/size-type/size-type-add/size-type-add.component';
import { SizeTypeListComponent } from './size-specification/size-type/size-type-list/size-type-list.component';


const specificationRoutes: Routes = [
    { path: 'General-Specification', component: GeneralSpecificationShellComponent },
    { path: 'Size-Specifcation', component: SizeSpecificationShellComponent },
  ];
  
  @NgModule({
    imports: [
      SharedModule,
      RouterModule.forChild(specificationRoutes),
      TabsModule,
      NgSelectModule,
    ],
    declarations: [
        GeneralSpecificationShellComponent,
        SizeSpecificationShellComponent,
        ColorAddComponent,
        ColorListComponent,
        ProductFabricListComponent,
        FabricAddComponent,
        TagAddComponent,
        TagListComponent,
        MeasureDimensionListComponent,
        MeasureDimensionAddComponent,
        SizeTypeListComponent,
        SizeTypeAddComponent
    ],
    entryComponents: [
        ColorAddComponent,
        FabricAddComponent,
        TagAddComponent,
        MeasureDimensionAddComponent,
        SizeTypeAddComponent
    ]
  })
  export class SpecificationModule { }