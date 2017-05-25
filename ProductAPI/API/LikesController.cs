using Microsoft.AspNetCore.Mvc;
using ProductAPI.Service;

namespace ProductAPI.Controllers {

  //LIKES controller
  [Route("api/[controller]")]
  public class LikesController : Controller {
    private LikeService _likeService;

    public LikesController (LikeService likeService) {
      _likeService = likeService;
      }

    // We will use same method for Like and Unlike
    // POST api/likes/userId/platformId/imageId/true|false     
    [HttpPost("{userId}/{platformId}/{imageId}/{likes}")]
    public void Post (string userId, int platformId, int imageId, bool likes) {

      _likeService.UpdateLike(userId, platformId, imageId, likes);

      }

    }
  }
