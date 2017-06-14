using System;
using System.Collections.Generic;

namespace ProductAPI.Models.Responses {
  public class KOFProductResponse {
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<ProductData> Data { get; set; }
    }

  public class ProductData {
    public int Id { get; set; }
    public string Title { get; set; }
    public string ColorWay { get; set; }
    public string MainImageUrl { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Sku { get; set; }
    public string[] BrandCollection { get; set; }
    public string ThumbUrl { get; set; }

    }
  }
