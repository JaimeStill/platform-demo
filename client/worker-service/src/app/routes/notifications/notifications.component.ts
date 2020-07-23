import {
  Component,
  OnInit
} from '@angular/core';

import {
  Notification,
  NotificationBinDialog,
  NotificationService
} from 'api';

import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'notifications-route',
  templateUrl: 'notifications.component.html'
})
export class NotificationsComponent implements OnInit {
  constructor(
    private dialog: MatDialog,
    private router: Router,
    public notify: NotificationService
  ) { }

  private refresh = () => {
    this.notify.getUnreadNotifications();
    this.notify.getReadNotifications();
    this.notify.getNotificationCount();
  }

  ngOnInit() {
    this.refresh();
  }

  viewNotification = (notification: Notification) => this.router.navigate(notification.url.split('/'));

  readNotification = async (notification: Notification) => {
    const res = await this.notify.toggleNotificationRead(notification);
    res && this.refresh();
  }

  viewNotificationBin = () => this.dialog.open(NotificationBinDialog, {
    autoFocus: false,
    width: '600px'
  })
  .afterClosed()
  .subscribe(() => this.refresh());
}
