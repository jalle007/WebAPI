using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public class LikeRepository : IGenericRepository<Like> {
    private ProductLikesContext _context;
    private DbSet<Like> likeEntity;
    public LikeRepository (ProductLikesContext context) {
      this._context = context;
      likeEntity = context.Set<Like>();
      }

    public Like GetSingle (int id) {
      return likeEntity.SingleOrDefault(s => s.LikeId == id);
      }

    public IQueryable<Like> GetAll () {
      return likeEntity.AsQueryable();
      }

    public void AddOrUpdate (Like like) {
      var any = _context.Like.Any(l => l.LikeId == like.LikeId);
      if (any) {
        _context.Entry(like).State = EntityState.Modified;
        } else {
        _context.Entry(like).State = EntityState.Added;
        }
      Save();
      }

    public void Save () {
      _context.SaveChanges();
      }

    }
  }
