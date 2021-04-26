import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../user/auth.service';
import { UserService } from '../user/user.service';
import { Menu } from './dashboard.component';

@Component({
  selector: 'pm-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  pageTitle = 'Admin';
  isLoggedIn = false;
  userName = "";
  rootMenu: Menu[];

  // get isLoggedIn(): boolean {
  //   return this.authService.isLoggedIn();
  // }


  constructor(private _userService: UserService,
    private authService: AuthService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem("userName") !== undefined && localStorage.getItem("userName") !== null) {
      this.isLoggedIn = true;
      //this.userName = JSON.parse(localStorage.getItem('userName'));
      this.userName = this.authService.getLogedUserName();
      this.getMenu(this.authService.getLogedUserId());
    }
    else
      this.router.navigate(['/login']);
  }

  logOut(): void {
    // comment
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getMenu(param) {
    this._userService.getUserPermissionforUser(param).subscribe(response => {
        if (response.state == 0)
            if (response.data != null && response.data.length > 0) {
                for (var i = 0; i < response.data.length; i++) {
                    if (response.data[i].isChecked == false)
                        continue;
                    if (this.rootMenu == undefined) {
                        this.rootMenu = [{
                            text: response.data[i].menuName, active: false,
                            Modules: this.getSubMenu(response.data[i].menuName, response.data[i].children),
                            icon:response.data[i].iconName
                        }
                        ];
                    }
                    else {
                        this.rootMenu.push({
                            text: response.data[i].menuName, active: false,
                            Modules: this.getSubMenu(response.data[i].menuName, response.data[i].children),
                            icon:response.data[i].iconName
                        }
                        );
                    }
                }
            }
    });
}

  getSubMenu(menu, params): any {
    let items: any[];
    for (var i = 0; i < params.length; i++) {
      if (params[i].isChecked == true) {
        if (items == undefined) {
          items = [{ path: menu + '/' + params[i].subMenuName, name: params[i].subMenuName }]
        }
        else {
          items.push(
            { path: menu + '/' + params[i].subMenuName, name: params[i].subMenuName }
          )
        }
      }
    }
    return items;
  }

  onClick(e, menu) {
    if (menu.active) {
      for (var i = 0; i < this.rootMenu.length; i++)
        this.rootMenu[i].active = false;
    }
    else {
      for (var i = 0; i < this.rootMenu.length; i++)
        this.rootMenu[i].active = false;
      menu.active = true;
    }
  }
  OnAway(e, menu) {
    //menu.active = false;
  }
}
