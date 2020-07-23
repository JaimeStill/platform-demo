namespace PlatformDemo.Data.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public string State { get; set; }
        public string EntityType { get; set; }
        public string Entity { get; set; }
        public string AuditDate { get; set; }
    }
}