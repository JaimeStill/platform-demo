import {
  Component,
  OnInit,
  OnDestroy
} from '@angular/core';

import {
  NotificationService,
  SocketService
} from 'api';

import { ThemeService } from 'core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {
  private subs = new Array<Subscription>();
  notifyCount: number;

  constructor(
    public notify: NotificationService,
    public socket: SocketService,
    public themer: ThemeService
  ) { }

  ngOnInit() {
    this.notify.getNotificationCount();

    this.subs.push(
      this.socket.notify$.subscribe(res => res && this.notify.getNotificationCount()),
      this.socket.alertNotify$.subscribe(res => res && this.notify.getNotificationCount()),
      this.notify.count$.subscribe(count => this.notifyCount = count)
    );
  }

  ngOnDestroy() {
    this.subs.forEach(sub => sub.unsubscribe());
  }
}
