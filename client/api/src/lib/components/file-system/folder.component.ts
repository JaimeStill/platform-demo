import {
  Component,
  Input,
  Output,
  EventEmitter,
  DoCheck
} from '@angular/core';

import {
  FileDialog,
  FolderDialog
} from '../../dialogs';

import {
  File,
  Folder
} from '../../models';

import { ConfirmDialog } from 'core';
import { MatDialog } from '@angular/material/dialog';
import { FileSystemService } from '../../services';

@Component({
  selector: 'folder',
  templateUrl: 'folder.component.html',
  providers: [FileSystemService]
})
export class FolderComponent implements DoCheck {
  @Input() folder: Folder;
  @Input() expanded = false;
  @Output() edit = new EventEmitter<Folder>();
  @Output() delete = new EventEmitter<Folder>();

  hovered = false;

  constructor(
    private dialog: MatDialog,
    public fileSystem: FileSystemService
  ) { }

  ngDoCheck() {
    if (this.expanded && !this.folder.loaded) this.loadFolder();
  }

  toggleExpanded = () => this.expanded
    ? this.expanded = false
    : this.loadFolder();

  toggleHovered = () => this.hovered = !this.hovered;

  loadFolder = async () => {
    this.expanded = true;
    const res = await this.fileSystem.getFolder(this.folder.id);
    this.fileSystem.getFolderFolders(this.folder.id);
    this.fileSystem.getFolderFiles(this.folder.id);

    if (res) {
      this.folder = res;
      this.folder.loaded = true;
    }
  }

  addFolder = () => this.dialog.open(FolderDialog, {
    data: { parentId: this.folder.id, color: 'black' } as Folder,
    disableClose: true,
    width: '800px'
  })
    .afterClosed()
    .subscribe(res => res && this.loadFolder());

  addFile = () => this.dialog.open(FileDialog, {
    data: { folderId: this.folder.id } as File,
    disableClose: true,
    width: '800px'
  })
    .afterClosed()
    .subscribe(res => res && this.loadFolder());

  editFolder = (folder: Folder) => this.dialog.open(FolderDialog, {
    data: Object.assign({} as Folder, folder),
    disableClose: true,
    width: '800px'
  })
    .afterClosed()
    .subscribe(res => res && this.loadFolder());

  editFile = (file: File) => this.dialog.open(FileDialog, {
    data: Object.assign({} as File, file),
    disableClose: true,
    width: '800px'
  })
    .afterClosed()
    .subscribe(res => res && this.loadFolder());

  deleteFolder = (folder: Folder) => this.dialog.open(ConfirmDialog, {
    data: {
      header: `Delete Folder: ${folder.name}?`,
      body: `Are you sure you want to delete the folder ${folder.name}?`
    },
    disableClose: true
  })
    .afterClosed()
    .subscribe(async (result) => {
      if (result) {
        const res = await this.fileSystem.removeFolder(folder);
        res && this.loadFolder();
      }
    });

  deleteFile = (file: File) => this.dialog.open(ConfirmDialog, {
    data: {
      header: `Delete File: ${file.name}?`,
      body: `Are you sure you want to delete the file ${file.name}?`
    },
    disableClose: true
  })
    .afterClosed()
    .subscribe(async (result) => {
      if (result) {
        const res = await this.fileSystem.removeFile(file);
        res && this.loadFolder();
      }
    });
}
