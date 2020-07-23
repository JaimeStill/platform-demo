import {
  Injectable,
  Optional
} from '@angular/core';

import { SnackerService } from 'core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ApiConfig } from '../config';
import { Notification } from '../models';
import { SocketService } from './sockets';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private count = new BehaviorSubject<number>(null);
  private notifications = new BehaviorSubject<Notification[]>(null);
  private unread = new BehaviorSubject<Notification[]>(null);
  private read = new BehaviorSubject<Notification[]>(null);

  count$ = this.count.asObservable();
  notifications$ = this.notifications.asObservable();
  unread$ = this.unread.asObservable();
  read$ = this.read.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    private socket: SocketService,
    @Optional() private config: ApiConfig
  ) { }

  getNotificationCount = () =>
    this.http.get<number>(`${this.config.api}notification/getNotificationCount`)
      .subscribe(
        data => this.count.next(data),
        err => this.snacker.sendErrorMessage(err.error)
      );

  getUnreadNotifications = () => this.http.get<Notification[]>(`${this.config.api}notification/getUnreadNotifications`)
    .subscribe(
      data => this.unread.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getReadNotifications = () => this.http.get<Notification[]>(`${this.config.api}notification/getReadNotifications`)
    .subscribe(
      data => this.read.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getDeletedNotifications = () => this.http.get<Notification[]>(`${this.config.api}notification/getDeletedNotifications`)
    .subscribe(
      data => this.notifications.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  addNotification = (notification: Notification): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}notification/addNotification`, notification)
      .subscribe(
        () => {
          this.socket.triggerNotification(notification.message);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  toggleNotificationRead = (notification: Notification): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}notification/toggleNotificationRead`, notification)
      .subscribe(
        () => {
          const message = notification.isRead
            ? `Notification marked unread`
            : `Notification marked read`;

          this.snacker.sendSuccessMessage(message);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  toggleNotificationDeleted = (notification: Notification): Promise<boolean> =>
    new Promise((resolve) => {
      this.http.post(`${this.config.api}notification/toggleNotificationDeleted`, notification)
        .subscribe(
          () => {
            const message = notification.isDeleted
              ? 'Notification successfully restored'
              : 'Notification successfully deleted';

            this.snacker.sendSuccessMessage(message);
            resolve(true);
          },
          err => {
            this.snacker.sendErrorMessage(err.error);
            resolve(false);
          }
        )
    });

  removeNotification = (notification: Notification): Promise<boolean> =>
    new Promise((resolve) => {
      this.http.post(`${this.config.api}notification/removeNotification`, notification)
        .subscribe(
          () => {
            this.snacker.sendSuccessMessage(`Notification permanently deleted`);
            resolve(true);
          },
          err => {
            this.snacker.sendErrorMessage(err.error);
            resolve(false);
          }
        )
    });
}
