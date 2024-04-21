using InvoicrCoreModels.Models.InvoiceEventLastProcessedEventModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace InvoicrInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
		private readonly IConfiguration _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json"). Build();
		public AppDbContext()
		{

		}

		// Constructor accepting DbContextOptions for normal usage
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Configure DbContext options from configuration
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}


		//DB sets
		// DbSet for InvoiceEventProcessLog
		public DbSet<InvoiceEventLastProcessedEvent> InvoiceEventProcessLogs { get; set; }
	}
}
