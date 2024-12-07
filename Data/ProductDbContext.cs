using Microsoft.EntityFrameworkCore;
using TH3.Models;

namespace TH3.Data
{
	public class ProductDbContext : DbContext
	{
		public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }
	}
}
