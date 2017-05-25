using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class ProductRepository : IGenericRepository<Product> {
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

    public void AddOrUpdate (Product entity) {
      var any = _context.Product.Any(item => item.ProductId == entity.ProductId);
      if (any) {
        _context.Entry(entity).State = EntityState.Modified;
        } else {
        _context.Entry(entity).State = EntityState.Added;
        }
      Save();
      }

    public void Save () {
      _context.SaveChanges();
      }


    }
  }
