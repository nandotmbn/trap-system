using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
	public static class ScopeContainer
	{
		public static IServiceCollection ScopeService(this IServiceCollection services, IConfiguration configuration)
		{
			// var channel = RedisChannel.Literal("SignalR");

			// services.AddSignalR();
			// services.AddSignalR().AddStackExchangeRedis(configuration["Redis:ConnectionString"]!, options =>
			// {
			// 	options.Configuration.ChannelPrefix = channel;
			// 	options.Configuration.ConnectTimeout = 10000;
			// 	options.Configuration.SyncTimeout = 10000;
			// 	options.Configuration.KeepAlive = 180;
			// 	options.Configuration.DefaultDatabase = 0;
			// });

			services.AddScoped<IAuth, AuthRepository>();
			services.AddScoped<IMine, MineRepository>();
			services.AddScoped<ISubstation, SubstationRepository>();
			services.AddScoped<ITicket, TicketRepository>();
			services.AddScoped<IChat, ChatRepository>();
			services.AddScoped<IContentDelivery, ContentDeliveryRepository>();

			return services;
		}
	}
}

