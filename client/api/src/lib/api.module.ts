import {
  ModuleWithProviders,
  NgModule
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CoreModule, MaterialModule } from 'core';

import { Components } from './components';
import { Dialogs } from './dialogs';

import { ApiConfig } from './config';

@NgModule({
  declarations: [
    ...Components,
    ...Dialogs
  ],
  entryComponents: [
    ...Dialogs
  ],
  imports: [
    CommonModule,
    CoreModule,
    MaterialModule,
    FormsModule,
    HttpClientModule
  ],
  exports: [
    ...Components,
    ...Dialogs
  ]
})
export class ApiModule {
  static forRoot(config: ApiConfig): ModuleWithProviders<ApiModule> {
    return {
      ngModule: ApiModule,
      providers: [
        { provide: ApiConfig, useValue: config }
      ]
    };
  }
}
