using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class PlatformRepository : IGenericRepository<Platform> {
    private ProductLikesContext _context;
    private DbSet<Platform> entity;
    public PlatformRepository (ProductLikesContext context) {
      this._context = context;
      entity = context.Set<Platform>();
      }

    public bool Exists (int id) {
      return  entity.Any(s => s.PlatformId == id);
      }


    public Platform GetSingle (int id) {
      return entity.SingleOrDefault(s => s.PlatformId == id);
      }

    public IQueryable<Platform> GetAll () {
      return entity.AsQueryable();
      }

    public void Save (Platform Platform) {
      _context.Entry(Platform).State = EntityState.Added;
      _context.SaveChanges();
      }

    public void AddOrUpdate (Platform entity) {
      var any = _context.Platform.Any(img => img.PlatformId == entity.PlatformId);
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
