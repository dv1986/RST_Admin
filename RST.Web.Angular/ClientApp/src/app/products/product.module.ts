import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { TabsModule } from 'ngx-bootstrap/tabs';

import { SharedModule } from '../shared/shared.module';
import { BrandAddComponent } from './product-brand/brand-add/brand-add.component';
import { BrandListComponent } from './product-brand/brand-list/brand-list.component';
import { CategoryAddComponent } from './product-category/category-add/category-add.component';
import { CategoryListComponent } from './product-category/category-list/category-list.component';

import { ProductCategoryShellComponent } from './product-category-shell.component';
import { ParentCategoryAddComponent } from './product-parent-category/parent-category-add/parent-category-add.component';
import { ParentCategoryListComponent } from './product-parent-category/parent-category-list/parent-category-list.component';
import { ParentSubCategoryAddComponent } from './product-parent-subcategory/parent-subcategory-add/parent-subcategory-add.component';
import { ParentSubCategoryListComponent } from './product-parent-subcategory/parent-subcategory-list/parent-subcategory-list.component';
import { SubCategoryAddComponent } from './product-subcategory/subcategory-add/subcategory-add.component';
import { SubCategoryListComponent } from './product-subcategory/subcategory-list/subcategory-list.component';
import { ManageProductFeatureShellComponent } from './manage-product-feature/manage-product-feature-shell.component';
import { ProductFeatureCategoryListComponent } from './manage-product-feature/product-feature-category/product-feature-category-list/product-feature-category-list.component';
import { ProductFeatureCategoryAddComponent } from './manage-product-feature/product-feature-category/product-feature-category-add/product-feature-category-add.component';
import { ProductFeaturesAddComponent } from './manage-product-feature/product-features/product-features-add/product-features-add.component';
import { ProductFeaturesListComponent } from './manage-product-feature/product-features/product-features-list/product-features-list.component';

import { TypeAddComponent } from './type/type-add/type-add.component';
import { TypeListComponent } from './type/type-list/type-list.component';
import { ProductTypeListComponent } from './product-type/product-type-list/product-type-list.component';
import { ProductTypeAddComponent } from './product-type/product-type-add/product-type-add.component';
import { ProductShellComponent } from './product/product-shell.component';
import { ProductListComponent } from './product/product-list/product-list.component';
import { ProductAddComponent } from './product/product-add/product-add.component';
import { ProductAttribteMappingListComponent } from './product-attribute-mapping-list/product-attribute-mapping-list.component';
import { ProductAttributeMappingAddComponent } from './product-attribute-mapping-add/product-attribute-mapping-add.component';
import { ProductImageMappingListComponent } from './product-images-mapping/product-images-mapping-list/product-images-mapping-list.component';
import { ProductImageMappingAddComponent } from './product-images-mapping/product-images-mapping-add/product-images-mapping-add.component';

const productRoutes: Routes = [
  { path: 'Manage-Categories', component: ProductCategoryShellComponent },
  { path: 'Manage-Features', component: ManageProductFeatureShellComponent },
  { path: 'Products', component: ProductShellComponent },
];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(productRoutes),
    TabsModule,
    NgSelectModule,
  ],
  declarations: [
    ProductCategoryShellComponent,
    ManageProductFeatureShellComponent,
    CategoryAddComponent,
    CategoryListComponent,
    TypeAddComponent,
    TypeListComponent,
    BrandAddComponent,
    BrandListComponent,
    ParentCategoryAddComponent,
    ParentCategoryListComponent,
    ParentSubCategoryListComponent,
    ParentSubCategoryAddComponent,
    SubCategoryAddComponent,
    SubCategoryListComponent,
    ProductFeatureCategoryListComponent,
    ProductFeatureCategoryAddComponent,
    ProductFeaturesAddComponent,
    ProductFeaturesListComponent,
    ProductTypeListComponent,
    ProductTypeAddComponent,
    ProductShellComponent,
    ProductListComponent,
    ProductAddComponent,
    ProductAttribteMappingListComponent,
    ProductAttributeMappingAddComponent,
    ProductImageMappingAddComponent,
    ProductImageMappingListComponent
  ],
  entryComponents: [
    TypeAddComponent,
    CategoryAddComponent,
    SubCategoryAddComponent,
    BrandAddComponent,
    ParentCategoryAddComponent,
    ParentSubCategoryAddComponent,
    ProductFeatureCategoryAddComponent,
    ProductFeaturesAddComponent,
    ProductTypeAddComponent,
    ProductAttributeMappingAddComponent,
    ProductImageMappingAddComponent
  ]
})
export class ProductModule { }
