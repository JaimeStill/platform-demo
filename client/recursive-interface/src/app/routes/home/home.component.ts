import {
  Component,
  OnInit
} from '@angular/core';

import { MatDialog } from '@angular/material/dialog';

import {
  FileSystemService,
  Folder,
  FolderDialog
} from 'api';

import {
  ConfirmDialog,
  ThemeService
} from 'core';

@Component({
  selector: 'home-route',
  templateUrl: 'home.component.html',
  providers: [FileSystemService]
})
export class HomeComponent implements OnInit {
  constructor(
    private dialog: MatDialog,
    public fileSystem: FileSystemService,
    public themer: ThemeService
  ) { }

  ngOnInit() {
    this.fileSystem.getRootFolders();
  }

  addFolder = () => this.dialog.open(FolderDialog, {
      data: { } as Folder,
      disableClose: true,
      width: '800px'
  })
  .afterClosed()
  .subscribe(res => res && this.fileSystem.getRootFolders());

  editFolder = (folder: Folder) => this.dialog.open(FolderDialog, {
      data: Object.assign({} as Folder, folder),
      disableClose: true,
      width: '800px'
  })
  .afterClosed()
  .subscribe(res => res && this.fileSystem.getRootFolders());

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
          res && this.fileSystem.getRootFolders();
      }
  });
}
