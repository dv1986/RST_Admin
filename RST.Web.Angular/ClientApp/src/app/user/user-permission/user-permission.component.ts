import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { TreeviewItem, TreeviewConfig, TreeItem } from 'ngx-treeview';
import { UserService } from '../user.service';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { AuthService } from '../auth.service';

@Component({
    selector: 'app-user-permission',
    templateUrl: './user-permission.component.html'
})
export class UserPermissionComponent implements OnInit, OnDestroy {
    pageTitle: string;
    items: TreeviewItem[];
    selectedIds: [];
    config = TreeviewConfig.create({
        hasAllCheckBox: true,
        hasFilter: true,
        hasCollapseExpand: true,
        decoupleChildFromParent: false,
        maxHeight: 400
    });
    userId: number;

    constructor(private _userService: UserService,
        private notificationService: NotificationService,
        private _authService: AuthService
    ) {

    }

    ngOnInit(): void {
        this.pageTitle = "User's Permissions";
        if (localStorage.getItem('selecteduserId')) {
            this.userId = JSON.parse(localStorage.getItem("selecteduserId"));
            this.AllPermissions(this.userId);
        }
        else {
            this.notificationService.ShowError("Please select a user from User-Management to assign permissions!")
        }
    }

    ngOnDestroy() {
        localStorage.removeItem("selecteduserId");
    }

    AllPermissions(param) {
        this._userService.getUserPermissionforUser(param).subscribe(response => {
            if (response.state == 0)
                if (response.data != null && response.data.length > 0) {
                    for (var i = 0; i < response.data.length; i++) {
                        if (this.items == undefined) {
                            this.items = [
                                new TreeviewItem({
                                    text: response.data[i].menuName, value: response.data[i].menuId, collapsed: false,
                                    children: this.getTreeNode(response.data[i].children)
                                })
                            ];
                        }
                        else {
                            this.items.push(
                                new TreeviewItem({
                                    text: response.data[i].menuName, value: response.data[i].menuId, collapsed: false,
                                    children: this.getTreeNode(response.data[i].children)
                                })
                            );
                        }
                    }
                }
        });
    }


    getTreeNode(params): any {
        let items: TreeItem[];
        for (var i = 0; i < params.length; i++) {
            if (items == undefined) {
                items = [{ text: params[i].subMenuName, value: params[i].subMenuId, checked: params[i].isChecked }]
            }
            else {
                items.push(
                    { text: params[i].subMenuName, value: params[i].subMenuId, checked: params[i].isChecked }
                )
            }
        }
        return items;
    }


    onSelectionChange(params) {
        this.selectedIds = params;
    }

    onUpdatePermissions() {
        this._userService.addUserPermission(this.userId, this.selectedIds).subscribe(response => {
            if (response.state == 0) {
                this.notificationService.ShowSuccess("Permission updated sucessfully!")
            }
        });
    }

    getBooks(): TreeviewItem[] {
        const childrenCategory = new TreeviewItem({
            text: 'Children', value: 1, collapsed: true, children: [
                { text: 'Baby 3-5', value: 11 },
                { text: 'Baby 6-8', value: 12 },
                { text: 'Baby 9-12', value: 13 }
            ]
        });
        const itCategory = new TreeviewItem({
            text: 'IT', value: 9, children: [
                {
                    text: 'Programming', value: 91, children: [{
                        text: 'Frontend', value: 911, children: [
                            { text: 'Angular 1', value: 9111 },
                            { text: 'Angular 2', value: 9112 },
                            { text: 'ReactJS', value: 9113, disabled: true }
                        ]
                    }, {
                        text: 'Backend', value: 912, children: [
                            { text: 'C#', value: 9121 },
                            { text: 'Java', value: 9122 },
                            { text: 'Python', value: 9123, checked: false, disabled: true }
                        ]
                    }]
                },
                {
                    text: 'Networking', value: 92, children: [
                        { text: 'Internet', value: 921 },
                        { text: 'Security', value: 922 }
                    ]
                }
            ]
        });
        const teenCategory = new TreeviewItem({
            text: 'Teen', value: 2, collapsed: true, disabled: true, children: [
                { text: 'Adventure', value: 21 },
                { text: 'Science', value: 22 }
            ]
        });
        const othersCategory = new TreeviewItem({ text: 'Others', value: 3, checked: false, disabled: true });
        return [childrenCategory, itCategory, teenCategory, othersCategory];
    }
}