import {
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';

import {
  Component,
  Inject
} from '@angular/core';

import { Alert } from '../../models';
import { AlertService } from '../../services';

@Component({
  selector: 'alert-dialog',
  templateUrl: 'alert.dialog.html',
  providers: [AlertService]
})
export class AlertDialog {
  constructor(
    private alertSvc: AlertService,
    private dialogRef: MatDialogRef<AlertDialog>,
    @Inject(MAT_DIALOG_DATA) public alert: Alert
  ) { }

  saveAlert = async () => {
    const res = this.alert.id > 0
      ? await this.alertSvc.updateAlert(this.alert)
      : await this.alertSvc.addAlert(this.alert);

    res && this.dialogRef.close(true);
  }
}
