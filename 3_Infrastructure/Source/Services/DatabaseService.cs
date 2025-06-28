using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
	public static class DatabaseContainer
	{
		public static IServiceCollection DatabaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<AppDBContext>
			(
				options =>
				{
					options.ConfigureWarnings(op => op.Ignore(RelationalEventId.PendingModelChangesWarning)).UseNpgsql(configuration.GetConnectionString("Default"),
					b => b.MigrationsAssembly("WebAPI"));
				},
				ServiceLifetime.Scoped
			);
			return services;
		}
	}
}

