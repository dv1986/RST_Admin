import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { Alert, AlertType } from '../../Model/Alert';

@Component({
    selector: 'app-cos-alert',
    templateUrl: './cos-alert.component.html',
    styleUrls: ['./cos-alert.component.css']
})
export class CosAlertComponent implements OnInit {
    isOpen = false;
    alert: Alert;
    constructor(private alertService: NotificationService) { }

    ngOnInit() {
        this.alertService.getAlert().subscribe((alert: Alert) => {
            if (!alert) {
                // clear alerts when an empty alert is received
                this.alert = null;
                return;
            }
            // add alert to array
            this.alert = alert;
            this.isOpen = true;
        });
    }
    cssClass() {
        if (!this.alert) {
            return;
        }
        // return css class based on alert type
        switch (this.alert.type) {
            case AlertType.Success:
                return 'success';
            case AlertType.Error:
                return 'danger';
            case AlertType.Info:
                return 'info';
            case AlertType.Warning:
                return 'warning';
        }
    }

    getTitle() {
        if (!this.alert) {
            return;
        }
        // return css class based on alert type
        switch (this.alert.type) {
            case AlertType.Success:
                return 'Success: ';
            case AlertType.Error:
                return 'Error: ';
            case AlertType.Info:
                return 'information: ';
            case AlertType.Warning:
                return 'Warning: ';
        }
    }
    onClosed() {
        console.log('closed');
        this.alert = null;
    }

}
