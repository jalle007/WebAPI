using System;
using System.Collections.Generic;

namespace ProductAPI.Models
{
    public partial class Image
    {
        public Image()
        {
            Like = new HashSet<Like>();
        }

        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string Picture { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }


        public virtual ICollection<Like> Like { get; set; }
        public virtual Product Product { get; set; }
    }
}
