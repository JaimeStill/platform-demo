import {
  Component,
  Input,
  Output,
  EventEmitter
} from '@angular/core';

import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { MatSliderChange } from '@angular/material/slider';

@Component({
  selector: 'datetime-picker',
  templateUrl: 'datetime-picker.component.html'
})
export class DatetimePickerComponent {
  private today = new Date(Date.now());

  _date = new Date(Date.now());
  hours: number;
  minutes: number;

  @Input()
  set date(date: string) {
    if (!date || !Date.parse(date)) {
      this._date = new Date(Date.now());
    } else {
      this._date = new Date(date);
    }

    this.minutes = this._date.getMinutes();
    this.hours = this._date.getHours();
    this.setDate(this._date);
  }

  get date(): string { return this._date.toUTCString() }

  @Input() label = 'Date Time';
  @Input() hint: string;
  @Input() hourMax = 23;
  @Input() hourMin = 0;
  @Input() hourStep = 1;
  @Input() hourThumb = true;
  @Input() hourInterval: number | string = 3;
  @Input() minuteMax = 59;
  @Input() minuteMin = 0;
  @Input() minuteStep = 1;
  @Input() minuteThumb = true;
  @Input() minuteInterval: number | string = 5;

  @Output() change = new EventEmitter<string>();

  dateChanged = (event: MatDatepickerInputEvent<Date | string>) => {
    event.value
      ? this.setDate(new Date(event.value))
      : this.setDate(new Date(this.today));
  }

  setDate = (event: Date) => {
    this._date = new Date(this.date);
    this._date.setFullYear(event.getFullYear());
    this._date.setMonth(event.getMonth());
    this._date.setDate(event.getDate());
    this._date.setHours(this.hours);
    this._date.setMinutes(this.minutes);
    this.change.emit(this.date);
  }

  setHours = (event: MatSliderChange) => {
    this._date.setHours(event.value);
    this.hours = event.value;
    this.change.emit(this.date);
  }

  setMinutes = (event: MatSliderChange) => {
    this._date.setMinutes(event.value);
    this.minutes = event.value;
    this.change.emit(this.date);
  }
}
