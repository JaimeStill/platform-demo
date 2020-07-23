export interface Alert {
  id: number;
  years: number;
  months: number;
  days: number;
  hours: number;
  minutes: number;
  message: string;
  url: string;
  alertType: string;
  trigger: string;
  recurring: boolean;
}
