using System.Linq;
using ProductAPI.Models;

namespace ProductAPI.Repository {
  public interface ILikeRepository {
    IQueryable<Like> GetAll ();
    Like GetSingle (int Id);
     void AddOrUpdate(Like entity);
    void Save ();
    }
  }
