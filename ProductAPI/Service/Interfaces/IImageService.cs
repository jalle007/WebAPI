using System.Linq;
using ProductAPI.Models;

namespace ProductAPI.Service {
  public interface IImageService {
    IQueryable<Image> GetByProduct (int productCode, string listOrder);
    dynamic Get (int id);
     void AddOrUpdate(Image entity) ;
    }
  }
