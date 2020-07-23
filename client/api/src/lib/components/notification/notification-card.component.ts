import {
  Component,
  Input,
  Output,
  EventEmitter
} from '@angular/core';

import { Notification } from '../../models';

@Component({
  selector: 'notification-card',
  templateUrl: 'notification-card.component.html'
})
export class NotificationCardComponent {
  @Input() isRecycled = false;
  @Input() notification: Notification;
  @Output() view = new EventEmitter<Notification>();
  @Output() read = new EventEmitter<Notification>();
  @Output() delete = new EventEmitter<Notification>();
  @Output() remove = new EventEmitter<Notification>();
}
