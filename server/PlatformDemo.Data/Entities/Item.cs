using System.Collections.Generic;

namespace PlatformDemo.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category Category { get; set; }
    }
}