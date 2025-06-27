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
			services.AddScoped<IAuth, AuthRepository>();
			services.AddScoped<IUser, UserRepository>();
			services.AddScoped<IMine, MineRepository>();
			services.AddScoped<ISubstation, SubstationRepository>();
			services.AddScoped<ICamera, CameraRepository>();
			services.AddScoped<ITicket, TicketRepository>();
			services.AddScoped<IChat, ChatRepository>();
			services.AddScoped<IDetection, DetectionRepository>();
			services.AddScoped<IClassification, ClassificationRepository>();
			services.AddScoped<IContentDelivery, ContentDeliveryRepository>();

			return services;
		}
	}
}

