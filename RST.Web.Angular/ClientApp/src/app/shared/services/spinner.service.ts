import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs';
 

@Injectable({
  providedIn: 'root',
})
export class SpinnerService {
  public spinnerSubject: BehaviorSubject<any> = new BehaviorSubject<any>({show:false, message:""});
  constructor() { }


  show(message="") {
    this.spinnerSubject.next({show:true, message:message});
  }

hide() {
    this.spinnerSubject.next({show:false, message:""});
}

getMessage(): Observable<any> {
    return this.spinnerSubject.asObservable();
}

}
