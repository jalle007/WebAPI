using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Service;

// GETIMAGES controller
namespace ProductAPI.Controllers {
  [Route("api/[controller]")]
  public class GetImagesController : Controller {
    private ImageService _imageService;

    public GetImagesController (ImageService imageService) {
      _imageService = imageService;
      }

    //GET /api/getimages/1/popular
    [HttpGet("{productCode}/{listOrder}/{pageNo}/{pageSize}")]
    public JsonResult Get (int productCode, string listOrder, int pageNo, int pageSize) {
      int skip = (pageNo - 1) * pageSize;
      int total1 = _imageService.CountAll();

      var images = _imageService.GetByProduct(productCode, listOrder)
                                                   .Skip(skip)
                                                   .Take(pageSize);

      var response = new {
        Images = images,
        Paging = new {
          total = total1,
          limit = pageSize,
          offset = skip,
          returned = images.Count()
          },
        Error = false
        };
      return Json(response);
      }

    //GET /api/getimages/1
    [HttpGet("{id}")]
    public JsonResult Get (int id) {
      var response = _imageService.Get(id);
      return Json(response);
      }

    }
  }
