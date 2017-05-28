using System.Linq;
using ProductAPI.Models;
using ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ProductAPI.Service {
  public class ImageService : IImageService {
    private ImageRepository _imageRepository;
    private LikeRepository _likeRepository;
    private ProductRepository _productRepository;
    private ProductLikesContext _context;

    public ImageService (ImageRepository imageRepository, ProductRepository productRepository, LikeRepository likeRepository, ProductLikesContext context) {
      _imageRepository = imageRepository;
      _likeRepository = likeRepository;
      _productRepository = productRepository;
      _context = context;
      }

    public dynamic Get (int id) {
      var image = _imageRepository.GetSingle(id);
      var product = _productRepository.GetSingle(image.ProductId);
      var likes = _likeRepository.GetAll().Where(m => m.ImageId == id && m.Liked).Count();

      var response = new {
        Data = new {
          Picture = image.Picture,
          UserId = image.UserId,
          Likes = likes,
          ProductName = product.Name
          },
        Error = false
        };
      return response;
      }
        public void AddOrUpdate(Image image) {
            _imageRepository.AddOrUpdate(image);
      }

    public int CountAll () {
      return _imageRepository.GetAll().Count();
      }

    public IQueryable<Image> GetByProduct (int productCode, string listOrder) {

      var popularImages = (
                           from image in _imageRepository.GetAll()
                           join l in _likeRepository.GetAll() on image.ImageId equals l.ImageId into likes
                           where (image.ProductId == productCode)
                           select new { image, Count = likes.Where(like => like.Liked).Count() })
                           .OrderByDescending(like => like.Count);

      var chronologicalImages = (from l in _likeRepository.GetAll()
                                 join image in _imageRepository.GetAll() on l.ImageId equals image.ImageId
                                 where (image.ProductId == productCode)
                                 select new { image, l.Timestamp })
                                 .OrderByDescending(like => like.Timestamp);

      if (listOrder == "popular")
        return popularImages.Select(res => res.image).AsQueryable();
      if (listOrder == "chronological")
        return chronologicalImages.Select(res => res.image).AsQueryable();

      return null;
      }
    }
  }
