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

      public void AddOrUpdate(Image image) {
      var any=_context.Image.Any(img=>img.ImageId==image.ImageId);
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

    }
  }
