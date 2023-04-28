using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Data
{
	public class ApplicationDbContext :DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option ):base (option) 
        {
            
        }
        public DbSet<Villa> villas { get; set; }
	}
}
