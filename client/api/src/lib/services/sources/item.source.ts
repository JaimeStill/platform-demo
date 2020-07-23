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
import { Item } from '../../models';

@Injectable()
export class ItemSource extends QueryService<Item> implements DataSource<Item> {
  columns = ['category.name', 'name', 'description'];
  smallColumns = ['name'];

  constructor(
    protected http: HttpClient,
    protected snacker: SnackerService,
    @Optional() config?: ApiConfig
  ) {
    super(http, snacker);

    this.sort = {
      isDescending: false,
      propertyName: 'category.name'
    };

    this.baseUrl = `${config.api}item/queryItems`;
  }

  trackItems = (item: Item) => item.id;
}
