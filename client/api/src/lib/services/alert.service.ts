import {
  Injectable,
  Optional
} from '@angular/core';

import { SnackerService } from 'core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ApiConfig } from '../config';
import { Alert } from '../models';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ApiConfig
  ) { }

  private alerts = new BehaviorSubject<Alert[]>(null);
  private alert = new BehaviorSubject<Alert>(null);

  alerts$ = this.alerts.asObservable();
  alert$ = this.alert.asObservable();

  clearAlerts = () => this.alerts.next(null);

  getAlerts = () => this.http.get<Alert[]>(`${this.config.api}alert/getAlerts`)
    .subscribe(
      data => this.alerts.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  searchAlerts = (search: string) => this.http.get<Alert[]>(`${this.config.api}alert/searchAlerts/${search}`)
    .subscribe(
      data => this.alerts.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getAlert = (id: number): Promise<boolean> => new Promise((resolve) => {
    this.http.get<Alert>(`${this.config.api}alert/getAlert/${id}`)
      .subscribe(
        data => {
          this.alert.next(data);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  addAlert = (alert: Alert): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}alert/addAlert`, alert)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`Alert successfully created`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  updateAlert = (alert: Alert): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}alert/updateAlert`, alert)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`Alert successfully updated`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  removeAlert = (alert: Alert): Promise<boolean> =>
    new Promise((resolve) => {
      this.http.post(`${this.config.api}alert/removeAlert`, alert)
        .subscribe(
          () => {
            this.snacker.sendSuccessMessage(`Alert successfully deleted`);
            resolve(true);
          },
          err => {
            this.snacker.sendErrorMessage(err.error);
            resolve(false);
          }
        );
    });
}
