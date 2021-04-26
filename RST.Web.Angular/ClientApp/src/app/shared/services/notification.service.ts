import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { Alert, AlertType } from '../Model/Alert';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmDialogueComponent } from '../components/confirm-dialogue/confirm-dialogue.component';

@Injectable({
    providedIn: 'root',
})
export class NotificationService {

    bsModalRef: BsModalRef;
    private subject = new Subject<Alert>();
    private keepAfterRouteChange = false;
    constructor(private toastr: ToastrService, private router: Router, private bsModalService: BsModalService) {
        router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                if (this.keepAfterRouteChange) {
                    // only keep for a single route change
                    this.keepAfterRouteChange = false;
                } else {
                    // clear alert messages
                    this.aletrClear();
                    this.toastr.clear();
                }
            }
        });
    }
    private getAlertMessage(messages: string[]): string {
        let notificationMessage = "";
        if (messages.length > 1) {
            notificationMessage = "<ul>";
            for (let index = 0; index < messages.length; index++) {
                debugger;
                if (messages[index].trim() != "")
                    notificationMessage += `<li>${messages[index]}</li>`
            }
            notificationMessage += "</ul>";
        }
        else {
            notificationMessage = messages[0];
        }
        return notificationMessage;
    }
    getAlert(): Observable<any> {
        return this.subject.asObservable();
    }

    alertSuccess(message: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Success, message, keepAfterRouteChange);
    }

    alertError(message: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Error, message, keepAfterRouteChange);
    }

    alertErrors(messages: string[], keepAfterRouteChange = false) {
        if (messages.length > 0) { this.alert(AlertType.Error, this.getAlertMessage(messages), keepAfterRouteChange); }
    }

    alertInfo(message: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Info, message, keepAfterRouteChange);
    }

    alertWarn(message: string, keepAfterRouteChange = false) {
        this.alert(AlertType.Warning, message, keepAfterRouteChange);
    }

    alert(type: AlertType, message: string, keepAfterRouteChange = false) {
        this.keepAfterRouteChange = keepAfterRouteChange;
        this.subject.next(<Alert>{ type: type, message: message });
    }

    aletrClear(): any {
        // clear alerts
        this.subject.next();
    }

    ShowError(errormessage: string, timeout: number = 0) {

        this.toastr.error(errormessage, "Error", {
            closeButton: true,
            timeOut: timeout,
            enableHtml: true,
            disableTimeOut: (timeout > 0) ? false : true,
            positionClass: "toast-bottom-full-width"
        });

    }

    ShowSuccess(errormessage: string, timeout: number = 0) {

        this.toastr.success(errormessage, "Information", {
            closeButton: true,
            timeOut: timeout,
            enableHtml: true,
            disableTimeOut: (timeout > 0) ? false : true,
            positionClass: "toast-bottom-full-width"
        });

    }

    ShowWarning(errormessage: string, timeout: number = 0) {

        this.toastr.warning(errormessage, "Information", {
            closeButton: true,
            timeOut: timeout,
            enableHtml: true,
            disableTimeOut: (timeout > 0) ? false : true,
            positionClass: "toast-bottom-full-width"
        });

    }

    confirm(title: string, message: string, buttons: Alert[], type: AlertType): Observable<string> {
        const initialState = {
            title: title,
            message: message,
            buttons: buttons,
            answer: "",
            type: type
        };
        this.bsModalRef = this.bsModalService.show(ConfirmDialogueComponent, { initialState });

        return new Observable<string>(this.getConfirmSubscriber());
    }

    private getConfirmSubscriber() {
        return (observer) => {
            const subscription = this.bsModalService.onHidden.subscribe((reason: string) => {
                observer.next(this.bsModalRef.content.answer);
                observer.complete();
            });

            return {
                unsubscribe() {
                    subscription.unsubscribe();
                }
            };
        }
    }
    ProcessResponse(response: any) {
        debugger;
        let notificationMessage = "";
        if (response.messages && response.messages.length > 0) {
            //console.log(response.messages.length);
            if (response.messages.length > 1) {
                notificationMessage = "<ul>";
                for (let index = 0; index < response.messages.length; index++) {
                    if (response.messages[index].trim() != "")
                    notificationMessage += `<li>${response.messages[index]}</li>`
                }
                notificationMessage += "</ul>";
            }
            else //Only Single message
            {
                notificationMessage = response.messages[0];
            }
        }
        /*else
        {
          this.ShowError(response.message);
        }*/

        switch (response.state) {
            case 0: //General Success Just Ignore no Message to Display
                break;
            case 1: //Data Modification Change was Successfull, Disply Success Message to user 
                if (notificationMessage == "") {
                    notificationMessage = "Operation completed successfully.";
                }
                this.ShowSuccess(notificationMessage, 6000);
                break;
            case 2:
                if (notificationMessage == "") {
                    notificationMessage = "It seems something is not quite rigth with the last operation.";
                }
                this.ShowWarning(notificationMessage, 5000);
                break;
            case 3:
                if (notificationMessage == "") {
                    notificationMessage = "Oh snap! Something wrong had happened.";
                }
                this.ShowError(notificationMessage);
                break;
            case 4:
                if (notificationMessage == "") {
                    notificationMessage = "Oh snap! Something not validate had happened.";
                }
                this.ShowError(notificationMessage);
                break;
        }
    }
}
