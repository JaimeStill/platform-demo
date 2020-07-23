import { Folder } from './folder';

export interface File {
  id: number;
  folderId: number;
  creationTime: Date;
  length: number;
  name: string;

  folder: Folder;
}
