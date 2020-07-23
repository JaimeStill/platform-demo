import { Route } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotificationsComponent } from './notifications/notifications.component';

export const RouteComponents = [
  HomeComponent,
  NotificationsComponent
];

export const Routes: Route[] = [
  { path: 'home', component: HomeComponent },
  { path: 'notifications', component: NotificationsComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];
