import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../_shared/services/authentication.service';
import { Router, NavigationEnd, ActivatedRoute, NavigationStart, RoutesRecognized, RouteConfigLoadStart, RouteConfigLoadEnd, NavigationCancel, NavigationError } from '@angular/router';
import { Title } from '@angular/platform-browser/';
import { BreadCrumb } from './BreadCrumb';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  breadcrumbs$ = this.router.events
    .filter(event => event instanceof NavigationEnd)
    
    .map(event => this.buildBreadCrumb(this.activatedRoute.root));

  constructor(private authService: AuthenticationService, private router: Router, private titleService: Title
    , private activatedRoute: ActivatedRoute) { 
      
    }
  loggedUserName: string = '';
  moduleTitle = "Cos BCS";
  ngOnInit() {
    this.loggedUserName = this.authService.getLogedUser();
    //this.moduleTitle = this.titleService.getTitle();
  }
  logout() {
    this.authService.logout();
    //this.router.navigate(['/login']);
    window.location.href = "https://cosnetsso.cos.net.au/Account/SignOut";
  }
 
  buildBreadCrumb(route: ActivatedRoute, url: string = '',
    breadcrumbs: Array<BreadCrumb> = []): Array<BreadCrumb> {
    
    const ROUTE_DATA_BREADCRUMB = 'breadcrumb';
    let children: ActivatedRoute[] = route.children;
    if (children.length === 0) {
     
      return breadcrumbs;
    }
    
    for (let child of children) {   
      
      if (child.outlet !== 'primary') {
       
        continue;
      }
      if (!child.snapshot.data.hasOwnProperty(ROUTE_DATA_BREADCRUMB)) {
       
        return this.buildBreadCrumb(child, url, breadcrumbs);
      }
      if(child.routeConfig.children && child.routeConfig.children.length > 0)
      {
       
        return this.buildBreadCrumb(child, url, breadcrumbs);
      }
      let routeURL = child.snapshot.url.map(segment => segment.path).join('/');
     
      url += `/${routeURL}`;
      let breadcrumb: BreadCrumb = {
        label: child.snapshot.data[ROUTE_DATA_BREADCRUMB],        
        url: url
      };
     // console.log(' child.snapshot.data['title']')
      this.moduleTitle = child.snapshot.data['title'];
      this.titleService.setTitle("COSBCS - "+this.moduleTitle);
      breadcrumbs.push(breadcrumb);
      return this.buildBreadCrumb(child, url, breadcrumbs);
    }
    
    return breadcrumbs;
  }
}
