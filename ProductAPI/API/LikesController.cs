using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;

namespace ProductAPI.Controllers {

  //LIKES controller
  [Route("api/[controller]")]
  public class LikesController : Controller {
    private readonly ProductLikesContext _context;
    public LikesController (ProductLikesContext context) { _context = context; }

    //we will use same method for Like and Unlike
    // POST api/likes/userId/platformId/imageId/true|false     
    [HttpPost("{userId}/{platformId}/{imageId}/{likes}")]
    public void Post (string userId, int platformId, int imageId, bool likes) {

      //update existing like
      var myLike = (from like in _context.Like
                    where (like.UserId == userId) && (like.PlatformId == platformId) && (like.ImageId == imageId)
                    select like).FirstOrDefault();
      if (myLike != null) {
        myLike.Liked = likes;
        _context.SaveChanges();
        } 
          else // add new like
          {
        var newLike = new Like {
          UserId = userId,
          ImageId = imageId,
          PlatformId = platformId,
          Liked = true
          };
        _context.Like.Add(newLike);
        _context.SaveChanges();

        }
      }

    }
  }
