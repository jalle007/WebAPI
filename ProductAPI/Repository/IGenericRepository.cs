using System.Linq;

namespace ProductAPI.Repository {
  public interface IGenericRepository<T> where T : class {

 //   IQueryable<T> FindBy (Expression<Func<T, bool>> predicate);
   IQueryable<T> GetAll () ;
 //   void Add (T entity);
  //  void Delete (T entity);
  //  void Edit (T entity);111
    void AddOrUpdate(T entity) ;
    void Save ();
    }
  }
