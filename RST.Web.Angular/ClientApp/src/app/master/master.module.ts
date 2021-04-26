import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { CountryShellComponent } from './master-country/country-shell.component';
import { CountryListComponent } from './master-country/country-list/country-list.component';
import { CountryAddComponent } from './master-country/country-add/country-add.component';
import { CountrySearchComponent } from './master-country/country-search/country-search.component';
import { StateListComponent } from './master-state/state-list/state-list.component';
import { StateSearchComponent } from './master-state/state-search/state-search.component';
import { StateShellComponent } from './master-state/state-shell.component';
import { StateAddComponent } from './master-state/state-add/state-add.component';
import { CitySearchComponent } from './master-city/city-search/city-search.component';
import { CityListComponent } from './master-city/city-list/city-list.component';
import { CityShellComponent } from './master-city/city-shell-component';
import { CityAddComponent } from './master-city/city-add/city-add.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { MenuAddComponent } from './master-menu/menu-add/menu-add.component';
import { MenuListComponent } from './master-menu/menu-list/menu-list.component';
import { MenuShellComponent } from './master-menu/menu-shell.comonent';
import { SubMenuShellComponent } from './master-submenu/submenu-shell.component';
import { SubMenuAddComponent } from './master-submenu/submenu-add/submenu-add.component';
import { SubMenuListComponent } from './master-submenu/submenu-list/submenu-list.component';
import { EmailTemplateShellComponent } from './master-email-template/email-template-shell.component';
import { EmailTemplateAddComponent } from './master-email-template/email-template-add/email-template-add.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { SeoContentAddComponent } from './master-seo-content/seo-content-add/seo-content-add.component';
import { SeoContentShellComponent } from './master-seo-content/seo-content.shell.component';
import { SeoContentListComponent } from './master-seo-content/seo-content-list/seo-content-list.component';

const userRoutes: Routes = [
  { path: 'Country', component: CountryShellComponent },
  { path: 'State', component: StateShellComponent },
  { path: 'City', component: CityShellComponent },
  { path: 'Menu', component: MenuShellComponent },
  { path: 'Sub-Menu', component: SubMenuShellComponent },
  { path: 'email-templates', component: EmailTemplateShellComponent },
  { path: 'Seo-Content', component: SeoContentShellComponent },
];

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild(userRoutes),
    TabsModule,
    NgSelectModule,
    AngularEditorModule
  ],
  declarations: [
      CountryShellComponent,
      CountryListComponent,
      CountryAddComponent,
      CountrySearchComponent,
      StateListComponent,
      StateSearchComponent,
      StateAddComponent,
      StateShellComponent,
      CitySearchComponent,
      CityListComponent,
      CityShellComponent, 
      CityAddComponent,
      MenuShellComponent,
      MenuAddComponent,
      MenuListComponent,
      SubMenuShellComponent,
      SubMenuAddComponent,
      SubMenuListComponent,
      EmailTemplateShellComponent,
      EmailTemplateAddComponent,
      SeoContentAddComponent,
      SeoContentShellComponent,
      SeoContentListComponent
  ],
  entryComponents: [CountrySearchComponent, StateSearchComponent, CitySearchComponent, CountryAddComponent, 
    StateAddComponent, MenuAddComponent, SubMenuAddComponent,SeoContentAddComponent],
})
export class MasterModule { }
