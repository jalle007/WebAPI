using System.Linq;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public interface IProductRepository {
    IQueryable<Product> GetAll ();
    Product GetSingle (int Id);
    void Save (Product entity);
    }
  }
