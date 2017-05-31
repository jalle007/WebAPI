using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Repository;

// GETIMAGES controller
namespace ProductAPI.Controllers {
  [Route("api/[controller]")]
  public class GetImagesController : Controller {
    ProductLikesContext _context;
    private MyRepository _repository;

    public GetImagesController (ProductLikesContext context) {
      _context = context;
      _repository = new MyRepository(context);
      }

    //GET /api/getimages/1/popular
    [HttpGet("{productCode}/{listOrder}/{pageNo}/{pageSize}")]
    public JsonResult Get (int productCode, string listOrder, int pageNo, int pageSize) {
      int skip = (pageNo - 1) * pageSize;
      dynamic response;

      var result = _repository._images.GetByProduct(productCode, listOrder);
      if (result.Any()) {
        var total1 = result.Count();
        var images = _repository._images.GetByProduct(productCode, listOrder)
                                                     .Skip(skip)
                                                     .Take(pageSize);
        response = new {
          Images = images,
          Paging = new {
            total = total1,
            limit = pageSize,
            offset = skip,
            returned = images.Count()
            },
          Error = false
          };
        } else {

        response = new {
          Error = true
          };
        }

      return Json(response);
      }

    //GET /api/getimages/1
    [HttpGet("{id}")]
    public JsonResult Get (int id, string userId = "") {
      var image = _repository._images.GetSingle(id);
      var product = _repository._products.GetSingle(image.ProductId);
      var likes = _repository._likes.GetAll().Where(m => m.ImageId == id && m.Liked).Count();

      bool isLiked = false;
      if (userId != "") {
        var res = _repository._likes.GetAll().Where(m => m.ImageId == id && m.UserId == userId);
        if (res.Count() > 0)
          isLiked = res.FirstOrDefault().Liked;
        }

      var response = new {
        Data = new {
          Picture = image.Picture,
          UserId = image.UserId,
          Likes = likes,
          ProductName = product.Name,
          Title = image.Title,
          Description = image.Description,
          Liked = isLiked
          },
        Error = false
        };
      return Json(response);
      }

    }
  }
