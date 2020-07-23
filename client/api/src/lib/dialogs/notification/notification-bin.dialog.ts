import {
  Component,
  OnInit
} from '@angular/core';

import { Notification } from '../../models';
import { NotificationService } from '../../services';

@Component({
  selector: 'notification-bin-dialog',
  templateUrl: 'notification-bin.dialog.html'
})
export class NotificationBinDialog implements OnInit {
  constructor(
    public notify: NotificationService
  ) { }

  ngOnInit() {
    this.notify.getDeletedNotifications();
  }

  restore = async (n: Notification) => {
    const res = await this.notify.toggleNotificationDeleted(n);
    res && this.notify.getDeletedNotifications();
  }

  remove = async (n: Notification) => {
    const res = await this.notify.removeNotification(n);
    res && this.notify.getDeletedNotifications();
  }
}
