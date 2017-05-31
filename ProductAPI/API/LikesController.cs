using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers {

  //LIKES controller
  [Route("api/[controller]")]
  public class LikesController : Controller {
    private MyRepository _repository;

    public LikesController (ProductLikesContext context) {
      _repository = new MyRepository(context);
      }

    // We will use same method for Like and Unlike
    // POST api/likes/userId/platformId/imageId/true|false     
    [HttpPost("{userId}/{platformId}/{imageId}/{likes}")]
    public JsonResult Post (string userId, int platformId, int imageId, bool likes) {
      var myLike = (from like in _repository._likes.GetAll()
                    where (like.UserId == userId) && (like.PlatformId == platformId) && (like.ImageId == imageId)
                    select like).FirstOrDefault();

      if (myLike != null) { //update like
        myLike.Liked = likes;
        _repository._likes.AddOrUpdate(myLike);
        }
        // add new like
        else {
        var newLike = new Like {
          UserId = userId,
          ImageId = imageId,
          PlatformId = platformId,
          Liked = true
          };
        _repository._likes.AddOrUpdate(newLike);

        }

      var response = new {
        Data = new {
          ImageId = imageId,
          UserId = userId,
          Liked = myLike == null ? true : likes
          },
        Error = false
        };
      return Json(response);

      }
    }
  }
