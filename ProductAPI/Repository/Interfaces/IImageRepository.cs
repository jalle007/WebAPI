using System.Linq;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public interface IImageRepository {
  //  IQueryable<Image> FindBy (Expression<Func<Image, bool>> predicate);
   // IQueryable<ImageView> GetByUserId(string userId);
    IQueryable<Image> GetAll ();
    IQueryable<Image> GetByProduct (int productCode, string listOrder) ;
    Image GetSingle (int Id);
    void AddOrUpdate(Image image);
    void Save ();
    }
  }
