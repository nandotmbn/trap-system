using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Messages;

namespace Infrastructure.Services
{
	public static class ConsumerContainer
	{
		public static IServiceCollection ConsumerService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHostedService<ClassificationMessageQuery>();

			return services;
		}
	}
}

