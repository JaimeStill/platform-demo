export interface Notification {
  id: number;
  message: string;
  url: string;
  pushDate: string;
  isAlert: boolean;
  isRead: boolean;
  isDeleted: boolean;
}
