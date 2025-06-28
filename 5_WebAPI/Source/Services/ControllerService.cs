using WebAPI.Middlewares;
using System.Text.Json.Serialization;

namespace WebAPI.Services
{
	public static class ControllerContainer
	{
		public static IServiceCollection ControllerServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers(options =>
			{
				options.Filters.Add<ExceptionWrapper>();
			});


			services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});
			return services;
		}
	}
}

