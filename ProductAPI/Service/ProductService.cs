using ProductAPI.Models;
using ProductAPI.Repository;

namespace ProductAPI.Service {
  public class ProductService : IProductService {
    private ImageRepository _imageRepository;
    private LikeRepository _likeRepository;
    private ProductRepository _productRepository;
    private ProductLikesContext _context;

    public ProductService (ImageRepository imageRepository, ProductRepository productRepository, LikeRepository likeRepository, ProductLikesContext context) {
      _imageRepository = imageRepository;
      _likeRepository = likeRepository;
      _productRepository = productRepository;
      _context = context;
      }

    public dynamic Get (int id) {

      return null;
      }

    }
  }
