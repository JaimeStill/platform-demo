import {
  MatDialogRef,
  MAT_DIALOG_DATA
} from '@angular/material/dialog';

import {
  Component,
  Inject,
  ViewChild,
  ElementRef,
  OnDestroy
} from '@angular/core';

import { CoreService } from 'core';
import { Subscription } from 'rxjs';
import { Folder } from '../../models';
import { FileSystemService } from '../../services';

@Component({
  selector: 'folder-dialog',
  templateUrl: 'folder.dialog.html',
  providers: [FileSystemService]
})
export class FolderDialog implements OnDestroy {
  private sub: Subscription;
  private initialized = false;
  validName = true;

  constructor(
    private core: CoreService,
    private dialogRef: MatDialogRef<FolderDialog>,
    public fileSystem: FileSystemService,
    @Inject(MAT_DIALOG_DATA) public folder: Folder
  ) { }

  @ViewChild('folderInput', { static: false })
  set folderInput(input: ElementRef) {
    if (input && !this.initialized) {
      this.sub = this.core.generateInputObservable(input)
        .subscribe(async val => {
          this.folder.name = val;
          this.validName = await this.fileSystem.validateFolderName(this.folder);
        });

      this.initialized = true;
    }
  }

  ngOnDestroy() {
    this.sub && this.sub.unsubscribe();
  }

  saveFolder = async () => {
    const res = this.folder.id > 0
      ? this.fileSystem.updateFolder(this.folder)
      : this.fileSystem.addFolder(this.folder);

    res && this.dialogRef.close(true);
  }
}
