using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class ImageRepository : IGenericRepository<Image> {
    private ProductLikesContext _context;
    private DbSet<Image> imageEntity;
    public ImageRepository (ProductLikesContext context) {
      this._context = context;
      imageEntity = context.Set<Image>();
      }

    public Image GetSingle (int id) {
      return imageEntity.SingleOrDefault(s => s.ImageId == id);
      }

    public void AddOrUpdate (Image image) {
      var any = _context.Image.Any(img => img.ImageId == image.ImageId);
      if (any) {
        _context.Entry(image).State = EntityState.Modified;
        } else {
        _context.Entry(image).State = EntityState.Added;
        }
      Save();
      }

    public void Save () {
      _context.SaveChanges();
      }

    public IQueryable<Image> GetAll () {
      return imageEntity.AsQueryable();
      }

    public IEnumerable<Image> GetByProduct (int productCode, string listOrder) {

      var popularImages = (
                           from image in _context.Image
                           join l in _context.Like on image.ImageId equals l.ImageId into likes
                           where (image.ProductId == productCode)
                           select new { image, Count = likes.Where(like => like.Liked).Count() })
                           .OrderByDescending(like => like.Count);

      var chronologicalImages = (from l in _context.Like
                                 join image in _context.Image on l.ImageId equals image.ImageId
                                 where (image.ProductId == productCode)
                                 select new { image, l.Timestamp })
                                 .OrderByDescending(like => like.Timestamp);

      if (listOrder == "popular")
        return popularImages.Select(res => res.image).AsEnumerable();
      if (listOrder == "chronological")
        return chronologicalImages.Select(res => res.image).AsEnumerable();

      return null;
      }

    public IEnumerable<Image> GetByProduct (string sku, string listOrder) {
      var product = _context.Product.Where(p => p.SKU == sku).FirstOrDefault();
      if (product == null) return null;

      var popularImages = (
                           from image in _context.Image
                           join l in _context.Like on image.ImageId equals l.ImageId into likes
                           where (image.ProductId == product.ProductId)
                           select new { image, Count = likes.Where(like => like.Liked).Count() })
                           .OrderByDescending(like => like.Count);

      var chronoImages = (from l in _context.Like
                                 join image in _context.Image on l.ImageId equals image.ImageId
                                 where (image.ProductId == product.ProductId)
                                 select new { image, l.Timestamp })
                                 .OrderByDescending(like => like.Timestamp);

      if (listOrder == "popular")
        return popularImages.Select(res => res.image).AsEnumerable();
      if (listOrder == "chronological")
        return chronoImages.Select(res => res.image).AsEnumerable();

      return null;
      }

    }
  }
