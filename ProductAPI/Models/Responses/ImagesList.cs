using System;
using System.Collections.Generic;

namespace ProductAPI.Models.Responses {
  public class ImagesList {
    public List<Image> images { get; set; }
    public Paging paging { get; set; }
    public bool error { get; set; }
    public string message { get; set; }
    }

  public class Paging {
    public int total { get; set; }
    public int limit { get; set; }
    public int offset { get; set; }
    public int returned { get; set; }
    }

  public class Image {
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
    public DateTime Timestamp { get; set; }

    }
  }
