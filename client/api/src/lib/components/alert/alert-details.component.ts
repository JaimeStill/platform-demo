import {
  Component,
  Input
} from '@angular/core';

import { Alert } from '../../models';

@Component({
  selector: 'alert-details',
  templateUrl: 'alert-details.component.html'
})
export class AlertDetailsComponent {
  @Input() alert: Alert;
  @Input() label: string;
}
