using System.Collections.Generic;

namespace PlatformDemo.Data.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public Folder Parent { get; set; }

        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
    }
}