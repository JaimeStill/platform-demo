import {
  Component,
  OnInit,
  OnDestroy
} from '@angular/core';

import { Subscription } from 'rxjs';

import {
  Item,
  ItemSource
} from 'api';

import {
  QueryResult,
  ThemeService
} from 'core';

@Component({
  selector: 'home-route',
  templateUrl: 'home.component.html',
  providers: [ItemSource]
})
export class HomeComponent implements OnInit, OnDestroy {
  private sub: Subscription;
  data: QueryResult<Item>;

  constructor(
    public itemSource: ItemSource,
    public themer: ThemeService
  ) { }

  ngOnInit() {
    this.sub = this.itemSource
      .queryResult$
      .subscribe((res: QueryResult<Item>) => this.data = res);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
