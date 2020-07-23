import { Item } from './item';

export interface Category {
  id: number;
  name: string;

  items: Item[];
}
