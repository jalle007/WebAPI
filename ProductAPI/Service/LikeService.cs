using ProductAPI.Models;
using ProductAPI.Repository;
using System.Linq;

namespace ProductAPI.Service {
  public class LikeService : ILikeService {
    private ImageRepository _imageRepository;
    private LikeRepository _likeRepository;
    private ProductRepository _productRepository;
    private ProductLikesContext _context;

    public LikeService (ImageRepository imageRepository, ProductRepository productRepository, LikeRepository likeRepository, ProductLikesContext context) {
      _imageRepository = imageRepository;
      _likeRepository = likeRepository;
      _productRepository = productRepository;
      _context = context;
      }

    public Like AddOrUpdate (Like entity) {
      _likeRepository.AddOrUpdate(entity);
      return null;
      }
    public void UpdateLike (string userId, int platformId, int imageId, bool likes) {

      var myLike = (from like in _likeRepository.GetAll()
                    where (like.UserId == userId) && (like.PlatformId == platformId) && (like.ImageId == imageId)
                    select like)
                           .FirstOrDefault();

      if (myLike != null)     //update like
      {
        myLike.Liked = likes;
        _likeRepository.AddOrUpdate(myLike);
        } 
        else                        // add new like
          {
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
