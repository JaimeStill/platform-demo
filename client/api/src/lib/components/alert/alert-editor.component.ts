import {
  Component,
  Input
} from '@angular/core';

import { MatSliderChange } from '@angular/material/slider';
import { Alert } from '../../models';

@Component({
  selector: 'alert-editor',
  templateUrl: 'alert-editor.component.html'
})
export class AlertEditorComponent {
  private _alert: Alert;

  private timeSpan: {
    years: number,
    months: number,
    days: number,
    hours: number,
    minutes: number
  };

  @Input()
  set alert(alert: Alert) {
    this._alert = alert;
    this.cacheTimeSpan(alert);
  }

  get alert(): Alert { return this._alert; }

  private cacheTimeSpan = (alert: Alert) => this.timeSpan = {
    years: alert.years,
    months: alert.months,
    days: alert.days,
    hours: alert.hours,
    minutes: alert.minutes
  }

  private clearTimeSpan = (alert: Alert) => {
    alert.years = 0;
    alert.months = 0;
    alert.days = 0;
    alert.hours = 0;
    alert.minutes = 0;
  }

  private restoreTimeSpan = (alert: Alert) => {
    alert.years = this.timeSpan.years;
    alert.months = this.timeSpan.months;
    alert.days = this.timeSpan.days;
    alert.hours = this.timeSpan.hours;
    alert.minutes = this.timeSpan.minutes;
  }

  setTrigger = (event: string) => this.alert.trigger = event;
  setYears = (event: MatSliderChange) => this.alert.years = event.value;
  setMonths = (event: MatSliderChange) => this.alert.months = event.value;
  setDays = (event: MatSliderChange) => this.alert.days = event.value;
  setHours = (event: MatSliderChange) => this.alert.hours = event.value;
  setMinutes = (event: MatSliderChange) => this.alert.minutes = event.value;

  toggleRecurring = () => {
    if (this.alert.recurring) {
      this.restoreTimeSpan(this.alert);
    } else {
      this.cacheTimeSpan(this.alert);
      this.clearTimeSpan(this.alert);
    }
  }
}
