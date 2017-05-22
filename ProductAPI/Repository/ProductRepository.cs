using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class ProductRepository : IProductRepository {
    private ProductLikesContext _context;
    private DbSet<Product> productEntity;
    public ProductRepository (ProductLikesContext context) {
      this._context = context;
      productEntity = context.Set<Product>();
      }


    public Product GetSingle (int id) {
      return productEntity.SingleOrDefault(s => s.ProductId == id);
      }

    public IQueryable<Product> GetAll () {
      return productEntity.AsQueryable();
      }

    public void Save (Product Product) {
      _context.Entry(Product).State = EntityState.Added;
      _context.SaveChanges();
      }


    }
  }
