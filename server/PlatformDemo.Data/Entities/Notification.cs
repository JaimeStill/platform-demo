namespace PlatformDemo.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
        public string PushDate { get; set; }
        public bool IsAlert { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
    }
}