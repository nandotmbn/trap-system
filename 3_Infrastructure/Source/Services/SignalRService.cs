using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Services
{
	public static class SignalRContainer
	{
		public static IServiceCollection SignalRService(this IServiceCollection services, IConfiguration configuration)
		{
			var channel = RedisChannel.Literal("SignalR");

			services.AddSignalR();
			services.AddSignalR().AddStackExchangeRedis(configuration["Redis:ConnectionString"]!, options =>
			{
				options.Configuration.ChannelPrefix = channel;
				options.Configuration.ConnectTimeout = 10000;
				options.Configuration.SyncTimeout = 10000;
				options.Configuration.KeepAlive = 180;
				options.Configuration.DefaultDatabase = 0;
			});

			return services;
		}
	}
}

