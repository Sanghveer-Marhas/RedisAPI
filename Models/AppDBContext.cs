using Microsoft.EntityFrameworkCore;

namespace RedisTestAPI.Models
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options)
			: base(options)
		{
		}

		public DbSet<Customers> customers { get; set; } = null!;
	}
}
