using System;

namespace PlatformDemo.Data.Entities
{
    public class File
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public DateTime CreationTime { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }

        public Folder Folder { get; set; }
    }
}