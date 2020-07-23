import { File } from './file';

export interface Folder {
  id: number;
  parentId: number;
  loaded: boolean;
  name: string;
  color: string;

  parent: Folder;

  files: File[];
  folders: Folder[];
}
