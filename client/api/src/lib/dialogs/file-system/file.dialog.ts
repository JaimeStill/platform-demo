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
import { File } from '../../models';
import { FileSystemService } from '../../services';

@Component({
  selector: 'file-dialog',
  templateUrl: 'file.dialog.html',
  providers: [FileSystemService]
})
export class FileDialog implements OnDestroy {
  private sub: Subscription;
  private initialized = false;
  validName = true;

  constructor(
    private core: CoreService,
    private dialogRef: MatDialogRef<FileDialog>,
    public fileSystem: FileSystemService,
    @Inject(MAT_DIALOG_DATA) public file: File
  ) { }

  @ViewChild('fileInput', { static: false })
  set fileInput(input: ElementRef) {
    if (input && !this.initialized) {
      this.sub = this.core.generateInputObservable(input)
        .subscribe(async val => {
          this.file.name = val;
          this.validName = await this.fileSystem.validateFileName(this.file);
        });

      this.initialized = true;
    }
  }

  ngOnDestroy() {
    this.sub && this.sub.unsubscribe();
  }

  saveFile = async () => {
    const res = this.file.id > 0
      ? this.fileSystem.updateFile(this.file)
      : this.fileSystem.addFile(this.file);

    res && this.dialogRef.close(true);
  }
}
