using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

// GETIMAGES controller
namespace ProductAPI.Controllers {
  [Route("api/[controller]")]
  public class GetImagesController : Controller {
    private readonly ProductLikesContext _context;
    public GetImagesController (ProductLikesContext context) {
      _context = context;
      }

    //GET /api/getimages/1/popular
    [HttpGet("{productCode}/{listOrder}/{pageNo}/{pageSize}")]
    public JsonResult Get (int productCode, string listOrder, int pageNo, int pageSize) {
      // Determine the number of records to skip
      int skip = (pageNo - 1) * pageSize;
      // Get total number of records
      int total1 = _context.Image.Count();

      var imageList = new List<Image>();

      var popularImages = (
                           from image in _context.Image
                           join l in _context.Like on image.ImageId equals l.ImageId into likes
                           where (image.ProductId == productCode)
                           select new { image, Count = likes.Where(like => like.Liked).Count() })
                           .OrderByDescending(like => like.Count)
                          .Skip(skip)
                          .Take(pageSize);

      var chronologicalImages = (from l in _context.Like
                                 join image in _context.Image on l.ImageId equals image.ImageId
                                 where (image.ProductId == productCode)
                                 select new { image, l.Timestamp })
                                 .OrderByDescending(like => like.Timestamp)
                                 .Skip(skip)
                                 .Take(pageSize);

      switch (listOrder) {
        case "popular":
        imageList = popularImages.Select(img => img.image).ToList();
        break;
        case "chronological":
        imageList = chronologicalImages.Select(img => img.image).ToList();
        break;
        default:
        break;
        }

      var response = new {
        Images = imageList,
        Paging = new {
          total = total1,
          limit = pageSize,
          offset = skip,
          returned = imageList.Count()
          },
        Error = false
        };
      return Json(response);
      }

    //GET /api/getimages/1
    [HttpGet("{id}")]
    public JsonResult Get (int id) {

      var image = _context.Image.SingleOrDefault(m => m.ImageId == id);
      var product = _context.Product.SingleOrDefault(p => p.ProductId == image.ProductId);
      var likes = _context.Like.Where(m => m.ImageId == id && m.Liked).Count();

      var response = new {
        Data = new {
          Picture = image.Picture,
          UserId = image.UserId,
          Likes = likes,
          ProductName = product.Name
          },
        Error = false
        };
      return Json(response);
      }

    // we will use this method for WebApp  to list all images
    //GET /api/getimages
    [HttpGet("user/{userId}")]
    public JsonResult Get (string userId) {

      var imageList = new List<Image>();

      var allImages = (from image in _context.Image
                       join l in _context.Like on image.ImageId equals l.ImageId into likes
                       join p in _context.Product on image.ProductId equals p.ProductId
                       select new ImageView {
                         image = image,
                         name = p.Name,
                         likes = likes.Where(like => like.Liked).Count(),
                         liked = likes.Where(l1 => (l1.UserId == userId) && (l1.ImageId == image.ImageId)).FirstOrDefault().Liked
                         }).OrderByDescending(like => like.likes);

      //var response = new {
      //  Images =   allImages,
      //  Error = false
      //  };
      return Json(allImages);
      }

    }
  }
