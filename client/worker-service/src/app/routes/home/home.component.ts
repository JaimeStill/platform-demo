import {
  Component,
  OnInit,
  OnDestroy
} from '@angular/core';

import {
  Alert,
  AlertDialog,
  AlertService,
  AlertSource,
  SocketService
} from 'api';

import {
  ConfirmDialog,
  QueryResult
} from 'core';

import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';

@Component({
  selector: 'home-route',
  templateUrl: 'home.component.html',
  providers: [AlertService, AlertSource]
})
export class HomeComponent implements OnInit, OnDestroy {
  private subs = new Array<Subscription>();
  data: QueryResult<Alert>;

  constructor(
    private dialog: MatDialog,
    private socket: SocketService,
    public alertSvc: AlertService,
    public alertSource: AlertSource
  ) { }

  ngOnInit() {
    this.alertSource.forceRefresh();

    this.subs.push(this.socket.alertNotify$.subscribe(trigger => {
      trigger && this.alertSource.forceRefresh();
    }));
  }

  ngOnDestroy() {
    this.subs.forEach(s => s.unsubscribe());
  }

  addAlert = () => this.dialog.open(AlertDialog, {
    data: { } as Alert,
    disableClose: true,
    width: '1200px'
  })
  .afterClosed()
  .subscribe(res => res && this.alertSource.forceRefresh());

  editAlert = (alert: Alert) => this.dialog.open(AlertDialog, {
    data: Object.assign({} as Alert, alert),
    disableClose: true,
    width: '1200px'
  })
  .afterClosed()
  .subscribe(res => res && this.alertSource.forceRefresh());

  removeAlert = (alert: Alert) => this.dialog.open(ConfirmDialog)
    .afterClosed()
    .subscribe(async result => {
      if (result) {
        const res = await this.alertSvc.removeAlert(alert);
        res && this.alertSource.forceRefresh();
      }
    });
}
