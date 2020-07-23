import {
  Injectable,
  Optional
} from '@angular/core';

import {
  QueryService,
  SnackerService
} from 'core';

import { DataSource } from '@angular/cdk/table';
import { HttpClient } from '@angular/common/http';
import { ApiConfig } from '../../config';
import { Alert } from '../../models';

@Injectable()
export class AlertSource extends QueryService<Alert> implements DataSource<Alert> {
  columns = ['id', 'message', 'trigger', 'recurring'];

  constructor(
    protected http: HttpClient,
    protected snacker: SnackerService,
    @Optional() private config: ApiConfig
  ) {
    super(http, snacker);
    this.sort = {
      isDescending: true,
      propertyName: 'trigger'
    };

    this.baseUrl = `${this.config.api}alert/queryAlerts`;
  }

  trackAlerts = (alert: Alert) => alert.id;
}
