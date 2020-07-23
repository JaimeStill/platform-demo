import {
  Injectable,
  Optional
} from '@angular/core';

import { SnackerService } from 'core';
import { BehaviorSubject } from 'rxjs';

import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel
} from '@microsoft/signalr';

import { ApiConfig } from '../../config';

@Injectable({
  providedIn: 'root'
})
export class SocketService {
  private endpoint = 'http://localhost:5000/core-socket';
  private connection: HubConnection;

  private connected = new BehaviorSubject<boolean>(false);
  private error = new BehaviorSubject<any>(null);
  private notify = new BehaviorSubject<boolean>(false);
  private alertNotify = new BehaviorSubject<boolean>(false);

  connected$ = this.connected.asObservable();
  error$ = this.error.asObservable();
  notify$ = this.notify.asObservable();
  alertNotify$ = this.alertNotify.asObservable();

  constructor(
    private snacker: SnackerService,
    @Optional() config?: ApiConfig
  ) {
    if (config) { this.endpoint = `${config.server}core-socket`; }

    this.connection = new HubConnectionBuilder()
      .withUrl(this.endpoint)
      .configureLogging(LogLevel.Debug)
      .withAutomaticReconnect()
      .build();

    this.connection
      .start()
      .then(() => this.connected.next(true))
      .catch((err) => {
        this.connected.next(false);
        this.error.next(err);
        this.snacker.sendErrorMessage(err.error);
      });

    this.connection.on(
      'receiveNotification',
      (message: string) => {
        this.snacker.sendColorMessage(message, ['snacker-teal']);
        this.notify.next(true);
      }
    );

    this.connection.on(
      'receiveAlertNotification',
      (message: string) => {
        this.snacker.sendColorMessage(message, ['snacker-indigo']);
        this.alertNotify.next(true);
      }
    );
  }

  triggerNotification = async (message: string) =>
    this.connected.value &&
    await this.connection
      .invoke('triggerNotification', message);
}
