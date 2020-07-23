import {
  FileDialog,
  FolderDialog
} from './file-system';

import { AlertDialog } from './alert/alert.dialog';
import { NotificationBinDialog } from './notification/notification-bin.dialog';

export const Dialogs = [
  AlertDialog,
  FileDialog,
  FolderDialog,
  NotificationBinDialog
];

export * from './file-system';
export * from './alert/alert.dialog';
export * from './notification/notification-bin.dialog';
