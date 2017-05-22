using System;
using System.Collections.Generic;

namespace ProductAPI.Models {
  public partial class Product {
    public Product () {
      Image = new HashSet<Image>();
      }

    public int ProductId { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Image> Image { get; set; }
    }
  }
