using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace ProductAPI.Models {
  public partial class Image {
    public Image () {
      Like = new HashSet<Like>();
      }

    public int ImageId { get; set; }
    public int ProductId { get; set; }
    public string Picture { get; set; }
    public string DeviceType { get; set; }
    public string DeviceId { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Username { get; set; }
    public string ProfilePicUrl { get; set; }
    // [NotMapped]
    //public string ProductSKU { get; set; }

    public virtual ICollection<Like> Like { get; set; }
    public virtual Product Product { get; set; }
    }
  }
