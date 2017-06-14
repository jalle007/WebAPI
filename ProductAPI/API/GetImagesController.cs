using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Models.Responses;
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

    //GET /api/getimages/popular|chronological/pageNo/pageSize
    [HttpGet("{listOrder}/{pageNo}/{pageSize}")]
    public JsonResult Get (string listOrder, int pageNo, int pageSize) {
      ImagesList response;

      if (listOrder != "popular" && listOrder != "chronological")
        return Json(new ImagesList() {
          error = true,
          message = "listOrder must be 'popular' or 'chronological'."
          });

      int skip = (pageNo - 1) * pageSize;

      var result = _repository._images.GetAll(listOrder);
      if (result != null) {
        var total1 = result.Count();
        var imagesList = _repository._images.GetAll(listOrder)
                                                         .Skip(skip)
                                                         .Take(pageSize)
                                                         .Select(img => new Models.Responses.Image {
                                                           Title = img.Title,
                                                           Description = img.Description,
                                                           Picture = img.Picture,
                                                           ImageId = img.ImageId,
                                                           DeviceId = img.DeviceId,
                                                           DeviceType = img.DeviceType,
                                                           ProductId = img.ProductId,
                                                           UserId = img.UserId,
                                                           Username = img.Username,
                                                           ProfilePicUrl = img.ProfilePicUrl
                                                           });

        //2
        response = new ImagesList() {
          images = imagesList.ToList(),
          paging = new Paging() {
            total = total1,
            limit = pageSize,
            offset = skip,
            returned = imagesList.Count()
            },
          error = false,
          message = ""
          };
        } else {
        response = new ImagesList() {
          error = true,
          message = "Error occured."
          };

        }
      var json = Json(response);
      return json;
      }


    //GET /api/getimages/sku/popular|chronological/pageNo/pageSize
    [HttpGet("{sku}/{listOrder}/{pageNo}/{pageSize}")]
    public JsonResult Get (string sku, string listOrder, int pageNo, int pageSize) {
      ImagesList response;
      if (listOrder != "popular" && listOrder != "chronological")
        return Json(new ImagesList() {
          error = true,
          message = "listOrder must be 'popular' or 'chronological'."
          });

      int skip = (pageNo - 1) * pageSize;

      var result = _repository._images.GetByProduct(sku, listOrder);
      if (result != null) {
        var total1 = result.Count();
        var imagesList = _repository._images.GetByProduct(sku, listOrder)
                                                         .Skip(skip)
                                                         .Take(pageSize)
                                                         .Select(img => new Models.Responses.Image {
                                                           Title = img.Title,
                                                           Description = img.Description,
                                                           Picture = img.Picture,
                                                           ImageId = img.ImageId,
                                                           DeviceId = img.DeviceId,
                                                           DeviceType = img.DeviceType,
                                                           ProductId = img.ProductId,
                                                           UserId = img.UserId,
                                                           Username = img.Username,
                                                           ProfilePicUrl = img.ProfilePicUrl
                                                           });

        //2
        response = new ImagesList() {
          images = imagesList.ToList(),
          paging = new Paging() {
            total = total1,
            limit = pageSize,
            offset = skip,
            returned = imagesList.Count()
            },
          error = false,
          message = ""
          };
        } else {
        response = new ImagesList() {
          error = true,
          message = "Error occured."
          };

        }
      var json = Json(response);
      return json;
      }

    //GET /api/getimages/1
    [HttpGet("{id}")]
    public JsonResult Get (int id, string userId = "") {
      var image = _repository._images.GetSingle(id);
      if (image == null)
        return Json(new {
          Error = true,
          Message = "image ID not found"
          });

      var product = _repository._products.GetSingle(image.ProductId);
      var likes = _repository._likes.GetAll().Where(m => m.ImageId == id && m.Liked).Count();

      KOFProductResponse productKOF = _repository._kixify.GetProductAsync(product.SKU).Result;

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
          Title = "",
          Id = 0,
          ProductSKU = product.SKU,
          Liked = isLiked,
          Username = image.Username,
          ProfilePicUrl = image.ProfilePicUrl
          },
        Error = false
        };

      if (productKOF.Data.FirstOrDefault() != null) {
        response.Data
                  .Set(x => x.Title, productKOF.Data.FirstOrDefault().Title)
                  .Set(x => x.Id, productKOF.Data.FirstOrDefault().Id);
        }
      return Json(response);
      }

    }
  }
