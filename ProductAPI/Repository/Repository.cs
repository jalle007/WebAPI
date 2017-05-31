using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class MyRepository {
    private ProductLikesContext _context;
    public ImageRepository _images;
    public ProductRepository _products;
    public LikeRepository _likes;
    public PlatformRepository _platform;


    public MyRepository (ProductLikesContext context) {
      this._context = context;

      _images = new ImageRepository(context);
      _products = new ProductRepository(context);
      _likes = new LikeRepository(context);
      _platform = new PlatformRepository(context);
      }


    }
  }
