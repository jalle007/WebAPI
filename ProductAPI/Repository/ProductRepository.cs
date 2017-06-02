using System;
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

      
    public bool Exists (int id) {
      return productEntity.Any(s => s.ProductId == id);
      }

      public bool Exists (string sku) {
    return productEntity.Any(s => s.SKU == sku);
    }

    public Product GetSingle (int id) {
      return productEntity.SingleOrDefault(s => s.ProductId == id);
      }

          public Product GetSingle (string sku) {
      return productEntity.SingleOrDefault(s => s.SKU == sku);
      }

    public IQueryable<Product> GetAll () {
      return productEntity.AsQueryable();
      }

    public void Save (Product Product) {
      _context.Entry(Product).State = EntityState.Added;
      _context.SaveChanges();
      }

    public void AddOrUpdate (Product entity) {
      throw new NotImplementedException();
      }

    public void Save () {
       _context.SaveChanges();
      }
    }
  }
