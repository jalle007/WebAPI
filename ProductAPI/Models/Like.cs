using System;
using System.Collections.Generic;

namespace ProductAPI.Models {
  public partial class Like {
    public int LikeId { get; set; }
    public string UserId { get; set; }
    public int PlatformId { get; set; }
    public int ImageId { get; set; }
    public bool Liked { get; set; }
    public DateTime Timestamp { get; set; }

    public virtual Image Image { get; set; }
    public virtual Platform Platform { get; set; }
    }
  }
