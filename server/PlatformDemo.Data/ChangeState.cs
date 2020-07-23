using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PlatformDemo.Data
{
    public class ChangeState
    {
        public List<EntityEntry> Added { get; set; }
        public List<EntityEntry> Modified { get; set; }
        public List<EntityEntry> Deleted { get; set; }
    }
}