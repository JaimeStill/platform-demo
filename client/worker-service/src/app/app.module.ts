import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { ApiModule } from 'api';
import { CoreModule, MaterialModule } from 'core';

import { AppComponent } from './app.component';

import { environment } from '../environments/environment';

import {
  Routes,
  RouteComponents
} from './routes';

@NgModule({
  declarations: [
    AppComponent,
    ...RouteComponents
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ApiModule.forRoot({ server: environment.server, api: environment.api }),
    CoreModule,
    MaterialModule,
    RouterModule.forRoot(Routes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
