import { Category } from './category';

export interface Item {
  id: number;
  categoryId: number;
  name: string;
  description: string;

  category: Category;
}
