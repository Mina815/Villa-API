using System.Linq.Expressions;
using Villa_API.Models;

namespace Villa_API.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null);
        Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool Tracked = true);
        Task Create(Villa Entity);
        Task Remove(Villa Entity);
        Task Save();
    }
}
