using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
	public class AppDBContext : DbContext
	{
		private readonly IConfiguration configuration;

		public AppDBContext(DbContextOptions options, IConfiguration _configuration) : base(options)
		{
			configuration = _configuration;
			ChangeTracker.LazyLoadingEnabled = true;
		}

		public required DbSet<User> Users { get; set; }
		public required DbSet<Substation> Substations { get; set; }
		public required DbSet<Camera> Cameras { get; set; }
		public required DbSet<Detection> Detections { get; set; }
		public required DbSet<Classification> Classifications { get; set; }
		public required DbSet<Ticket> Tickets { get; set; }
		public required DbSet<Chat> Chats { get; set; }
		public required DbSet<ContentDelivery> ContentDeliveries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			if (configuration["SuperUser:Id"] != null) {
				UserEntityBuilder.Set(modelBuilder, configuration);
			}

		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override async Task<int> SaveChangesAsync(bool success, CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(success, cancellationToken);
		}

		public override int SaveChanges()
		{
			return base.SaveChanges();
		}
	}
}

