import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { LayoutServiceService } from '../layout-service.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    public RootMenu: any;
    public Modules: any;
    constructor(private titleService: Title, private LayoutService: LayoutServiceService, private AuthService: AuthenticationService) {
        this.titleService.setTitle("COS BCS")
    }

    ngOnInit() {
        // this.LayoutService.LoadSystemConfigs().subscribe(response => {

        // });
        this.LayoutService.GetWebRootMenu().subscribe(response => {
            this.RootMenu = response.data;
        });
    }
    OnHover(e, menu) {
        this.LayoutService.GetChildModules(this.AuthService.getLogedUserName(), menu.id).subscribe(response => {
            for (var i = 0; i < this.RootMenu.length; i++)
                this.RootMenu[i].active = false;
            this.Modules = response.data;
            menu.active = true;
        });
    }
    OnAway(e, menu) {
        //menu.active = false;
    }
}
