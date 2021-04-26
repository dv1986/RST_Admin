import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ProductCategoryShellComponent } from '../products/product-category-shell.component';

import { SharedModule } from '../shared/shared.module';
import { CategoryAttributeMappingAddComponent } from './category-attribute-mapping/category-attribute-mapping-add/category-attribute-mapping-add.component';
import { CategoryAttributeMappingEditComponent } from './category-attribute-mapping/category-attribute-mapping-edit/category-attribute-mapping-edit.component';
import { CategoryAttributeMappingListComponent } from './category-attribute-mapping/category-attribute-mapping-list/category-attribute-mapping-list.component';
import { ManageProductAttributeShellComponent } from './manage-product-attribute-shell.component';
import { ProductAttributeParentAddComponent } from './product-attribute-parent/product-attribute-parent-add/product-attribute-parent-add.component';
import { ProductAttributeParentListComponent } from './product-attribute-parent/product-attribute-parent-list/product-attribute-parent-list.component';
import { ProductAttributeAddComponent } from './product-attribute/product-attribute-add/product-attribute-add.component';
import { ProductAttributeListComponent } from './product-attribute/product-attribute-list/product-attribute-list.component';


const productRoutes: Routes = [
  { path: 'Manage-Attributes', component: ManageProductAttributeShellComponent },
];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(productRoutes),
    TabsModule,
    NgSelectModule,
  ],
  declarations: [
    ManageProductAttributeShellComponent,
    ProductAttributeParentListComponent,
    ProductAttributeParentAddComponent,
    ProductAttributeListComponent,
    ProductAttributeAddComponent,
    CategoryAttributeMappingAddComponent,
    CategoryAttributeMappingListComponent,
    CategoryAttributeMappingEditComponent
  ],
  entryComponents: [
    ProductAttributeParentAddComponent,
    CategoryAttributeMappingEditComponent,
    ProductAttributeAddComponent,
    CategoryAttributeMappingAddComponent
  ]
})
export class AttributeModule { }
