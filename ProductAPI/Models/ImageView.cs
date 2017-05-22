using System;
using System.Collections.Generic;

namespace ProductAPI.Models {
  public partial class ImageView {
    public ImageView () {
      }
    public Image image { get; set; }
    public string name { get; set; }
    public int likes { get; set; }
    public bool? liked { get; set; }
    }
  }
