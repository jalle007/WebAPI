using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Controllers {

  //LIKES controller
  [Route("api/[controller]")]
  public class LikesController : Controller {
    private LikeRepository _likeRepository;

    public LikesController (ProductLikesContext context) {
      _likeRepository = new LikeRepository(context);
      }

    // We will use same method for Like and Unlike
    // POST api/likes/userId/platformId/imageId/true|false     
    [HttpPost("{userId}/{platformId}/{imageId}/{likes}")]
    public void Post (string userId, int platformId, int imageId, bool likes) {

      var myLike = (from like in _likeRepository.GetAll()
                    where (like.UserId == userId) && (like.PlatformId == platformId) && (like.ImageId == imageId)
                    select like).FirstOrDefault();

      if (myLike != null) 
      { //update like
        myLike.Liked = likes;
        _likeRepository.AddOrUpdate(myLike);
        }
        // add new like
        else {
        var newLike = new Like {
          UserId = userId,
          ImageId = imageId,
          PlatformId = platformId,
          Liked = true
          };
        _likeRepository.AddOrUpdate(newLike);

        }
      }
    }
  }
