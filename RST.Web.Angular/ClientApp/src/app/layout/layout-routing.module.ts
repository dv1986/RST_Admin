import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { PermissionResolverService } from '../_shared/guard/permission-resolver.service';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', },
      { path: 'dashboard', component: DashboardComponent, },
      {
        path: 'BidPricing', loadChildren: '../bid-pricing/bid-pricing.module#BidPricingModule',
        data: { breadcrumb: 'Bid Pricing ', title: 'Bid Pricing' }
      },
      {
        path: 'bin-management', loadChildren: '../bin-management/bin-management.module#BinManagementModule',
        data: { breadcrumb: 'Bin Management ', title: 'Bin Management', moduleName: "binmgtweb" },
        resolve: { permissions: PermissionResolverService }
      }
      , {
        path: 'print-management', loadChildren: '../print-management/print-management.module#PrintManagementModule',
        data: { breadcrumb: 'Print Management ', title: 'Print Management', moduleName: "" },
      }
      , {
        path: 'order-entry', loadChildren: '../order-entry/order-entry.module#OrderEntryModule',
        data: { breadcrumb: 'Order Entry ', title: 'Order Entry', moduleName: "ordrentry" },
        resolve: { permissions: PermissionResolverService }
      }
      , {
        path: 'order-entry-new', loadChildren: '../order-entry-new/order-entry-new.module#OrderEntryNewModule',
        data: { breadcrumb: 'Order Entry ', title: 'Order Entry', moduleName: "ordrentrynew" },
        resolve: { permissions: PermissionResolverService }
      }
      , {
        path: 'driver-management', loadChildren: '../driver-management/driver-management.module#DriverManagementModule',
        data: { breadcrumb: 'Driver Management ', title: 'Driver Management', moduleName: "" }
      }
      , {
        path: 'contract-management', loadChildren: '../contract-management/contract-management.module#ContractManagementModule',
        data: { breadcrumb: 'Contract Management ', title: 'Contract Management', moduleName: "" }
      }
      , {
        path: 'agreed-cost', loadChildren: '../agreed-cost/agreed-cost.module#AgreedCostModule',
        data: { breadcrumb: 'Agreed Cost ', title: 'Agreed Cost', moduleName: "" }
      }
      , {
        path: 'rep-budget', loadChildren: '../rep-budget/rep-budget.module#RepBudgetModule',
        data: { breadcrumb: 'Rep Budget ', title: 'Rep Budget', moduleName: "" }
      }, {
        path: 'discontinued-product', loadChildren: '../discontinued-product/discontinued-product.module#DiscontinuedProductModule',
        data: { breadcrumb: 'Discontinued Products ', title: 'Discontinued Products', moduleName: "" }
      },
      {
        path: 'order-staging', loadChildren: '../order-staging/order-staging.module#OrderStagingModule',
        data: { breadcrumb: 'Order Staging ', title: 'Order Staging', moduleName: "OrderStaging" },
        resolve: { permissions: PermissionResolverService }
      },
      {
        path: 'tech-isle', loadChildren: '../tech-isle/tech-isle.module#TechIsleModule',
        data: { breadcrumb: 'Tech Isle ', title: 'Tech Isle', moduleName: "" }
      },
      {
        path: 'bts-pack', loadChildren: '../bts-pack/bts-pack.module#BtsPackModule',
        data: { breadcrumb: 'Bts Pack', title: 'Bts Pack', moduleName: "BtsPack" },
        resolve: { permissions: PermissionResolverService }
      },
      {
        path: 'procurement', loadChildren: '../offshore-procurement/offshore-procurement.module#OffshoreProcurementModule',
        data: { breadcrumb: 'Procurement', title: 'Procurement', moduleName: "OffshoreProcurement" },
        resolve: { permissions: PermissionResolverService }
      },
      {
        path: 'category-management', loadChildren: '../category-management/category-management.module#CategoryManagementModule',
        data: { breadcrumb: 'Category Management', title: 'Category Management', moduleName: "CategoryManagement" },
        resolve: { permissions: PermissionResolverService }
      },
      {
        path: 'demo-module', loadChildren: '../demo-module/demo-module.module#DemoModuleModule',
        data: { breadcrumb: 'Demo Module', title: 'Demo', moduleName: "Demo Module" },
        resolve: { permissions: PermissionResolverService }
      },
      {
        path: 'cosnet', loadChildren: '../cosnet/cosnet.module#CosnetModule',
        data: { breadcrumb: 'Cosnet', title: 'Cosnet', moduleName: "Cosnet" },
        resolve: { permissions: PermissionResolverService }
      }, {
        path: 'retention', loadChildren: '../retention/retention.module#RetentionModule',
        data: { breadcrumb: 'Retention ', title: 'Retention', moduleName: "" },
      },
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]

})
export class LayoutRoutingModule { }
