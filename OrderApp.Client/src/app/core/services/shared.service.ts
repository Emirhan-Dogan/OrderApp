import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private subject = new Subject<any>();

  sendChangeUserNameEvent(userName: string) {
    this.subject.next(userName);  
}

  getChangeUserNameClickEvent():Observable<any>{
    return this.subject.asObservable();
 }

}
