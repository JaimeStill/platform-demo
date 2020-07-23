namespace PlatformDemo.Data.Entities
{
    public class Alert
    {
        public int Id { get; set; }
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string AlertType { get; set; }
        public string Trigger { get; set; }
        public bool Recurring { get; set; }
    }
}