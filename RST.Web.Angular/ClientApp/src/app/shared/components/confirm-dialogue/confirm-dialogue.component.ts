import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AlertType, Alert } from '../../Model/Alert';
@Component({
  selector: 'app-confirm-dialogue',
  templateUrl: './confirm-dialogue.component.html',
  styleUrls: ['./confirm-dialogue.component.css']
})
export class ConfirmDialogueComponent {

  title: string;
  message: string;
  buttons: Alert[] = [{ message: "No", type: AlertType.Default }, { message: "Yes", type: AlertType.Error }];
  answer: string = "";
  type: AlertType = AlertType.Warning;
  constructor(
    public bsModalRef: BsModalRef,
  ) {
    
  }

  respond(answer: string) {
    this.answer = answer;

    this.bsModalRef.hide();
  }

}

