using Microsoft.EntityFrameworkCore;
using Villa_API.Models;

namespace Villa_API.Data
{
	public class ApplicationDbContext :DbContext
	{
		public DbSet<Villa> villas;
	}
}
