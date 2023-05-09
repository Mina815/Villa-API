using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Villa_API.Data;
using Villa_API.Models;
using Villa_API.Repository.IRepository;

namespace Villa_API.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task Create(Villa Entity)
        {
            await _db.AddAsync(Entity);
            await Save();    
        }

        public async Task<Villa> Get(Expression<Func<Villa,bool>> filter = null, bool Tracked = true)
        {
            IQueryable<Villa> query = _db.villas;
            if(!Tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null)
        {
            IQueryable<Villa> query = _db.villas;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(Villa Entity)
        {
            _db.Remove(Entity);
            await Save();
        }

        public async Task Save()
        {
           await _db.SaveChangesAsync();
        }
    }
}
