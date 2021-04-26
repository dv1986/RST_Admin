import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {

  constructor( private router: Router,
    private activatedRoute: ActivatedRoute, ) {
    router.events.subscribe((val) => {
      if (val instanceof NavigationEnd) {
        if (val.url != "/login" && val.url != "/sso") {
          localStorage.setItem("cosbcslasturl", val.url);
        }
      }
    });


  }

  ngOnInit() {
   /*this.router.events
      .filter((event) => event instanceof NavigationEvent)
      .map(() => this.activatedRoute)
      .map((route) => {
        
        while (route.firstChild) route = route.firstChild;
        
        return route;
      })*/
  }
  
}
