import {
  Injectable,
  Optional
} from '@angular/core';

import {
  File,
  Folder
} from '../models';

import { SnackerService } from 'core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ApiConfig } from '../config';

@Injectable()
export class FileSystemService {
  private file = new BehaviorSubject<File>(null);
  private folder = new BehaviorSubject<Folder>(null);
  private files = new BehaviorSubject<File[]>(null);
  private folders = new BehaviorSubject<Folder[]>(null);

  file$ = this.file.asObservable();
  folder$ = this.folder.asObservable();
  files$ = this.files.asObservable();
  folders$ = this.folders.asObservable();

  constructor(
    private http: HttpClient,
    private snacker: SnackerService,
    @Optional() private config: ApiConfig
  ) { }

  getRootFolders = () => this.http.get<Folder[]>(`${this.config.api}folder/getRootFolders`)
    .subscribe(
      data => this.folders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getFolderFolders = (parentId: number) => this.http.get<Folder[]>(`${this.config.api}folder/getFolderFolders/${parentId}`)
    .subscribe(
      data => this.folders.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getFolder = (folderId: number): Promise<Folder> => new Promise((resolve) => {
    this.http.get<Folder>(`${this.config.api}folder/getFolder/${folderId}`)
      .subscribe(
        data => {
          this.folder.next(data);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  });

  validateFolderName = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post<boolean>(`${this.config.api}folder/validateFolderName`, folder)
      .subscribe(
        data => resolve(data),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  addFolder = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/addFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully created`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  updateFolder = (folder: Folder): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/updateFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully updated`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  removeFolder = (folder: Folder) => new Promise((resolve) => {
    this.http.post(`${this.config.api}folder/removeFolder`, folder)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${folder.name} successfully deleted`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  getFolderFiles = (folderId: number) => this.http.get<File[]>(`${this.config.api}file/getFolderFiles/${folderId}`)
    .subscribe(
      data => this.files.next(data),
      err => this.snacker.sendErrorMessage(err.error)
    );

  getFile = (fileId: number): Promise<File> => new Promise((resolve) => {
    this.http.get<File>(`${this.config.api}file/getFile/${fileId}`)
      .subscribe(
        data => {
          this.file.next(data);
          resolve(data);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(null);
        }
      );
  });

  validateFileName = (file: File): Promise<boolean> => new Promise((resolve) => {
    this.http.post<boolean>(`${this.config.api}file/validateFileName`, file)
      .subscribe(
        data => resolve(data),
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  addFile = (file: File): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}file/addFile`, file)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${file.name} successfully created`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  updateFile = (file: File): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`${this.config.api}file/updateFile`, file)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${file.name} successfully updated`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });

  removeFile = (file: File): Promise<boolean> => new Promise((resolve) => {
    this.http.post(`/api.file/removeFile`, file)
      .subscribe(
        () => {
          this.snacker.sendSuccessMessage(`${file.name} successfully deleted`);
          resolve(true);
        },
        err => {
          this.snacker.sendErrorMessage(err.error);
          resolve(false);
        }
      );
  });
}
