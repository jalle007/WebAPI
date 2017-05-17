using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Platform
    {
        public Platform()
        {
            Like = new HashSet<Like>();
        }

        public int PlatformId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Like> Like { get; set; }
    }
}
