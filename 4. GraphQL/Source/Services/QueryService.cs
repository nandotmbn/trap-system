
using GraphQL.Source;

namespace GraphQL.Services;

public static class QueryContainer
{
  public static IServiceCollection QueryService(this IServiceCollection services, IConfiguration configuration)
  {
    services
      .AddGraphQLServer()
      .AddQueryType<Query>()

      .AddTypeExtension<CameraQuery>()

      .AddTypeExtension<ChatQuery>()

      .AddTypeExtension<ClassificationQuery>()

      .AddTypeExtension<DetectionQuery>()

      .AddTypeExtension<SubstationQuery>()

      .AddTypeExtension<TicketQuery>()

      .AddTypeExtension<UserQuery>()
      .AddType<UserDescriptor>()

      .AddProjections()
      .AddFiltering()
      .AddSorting()
      .AddDbContextCursorPagingProvider();

    return services;
  }
} 