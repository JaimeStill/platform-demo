import {
  Component,
  Input,
  Output,
  EventEmitter
} from '@angular/core';

import { Alert } from '../../models';

@Component({
  selector: 'alert-card',
  templateUrl: 'alert-card.component.html'
})
export class AlertCardComponent {
  @Input() alert: Alert;
  @Input() editable = false;
  @Input() size = 320;
  @Output() edit = new EventEmitter<Alert>();
  @Output() remove = new EventEmitter<Alert>();
}
