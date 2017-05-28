using ProductAPI.Models;

namespace ProductAPI.Service {
  public interface ILikeService {

    Like AddOrUpdate(Like entity);
    void UpdateLike (string userId, int platformId, int imageId, bool likes) ;

    }
  }
