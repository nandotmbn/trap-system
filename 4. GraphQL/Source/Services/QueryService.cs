
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

      .AddTypeExtension<ContentDeliveryQuery>()

      .AddTypeExtension<DetectionQuery>()

      .AddTypeExtension<SubstationQuery>()

      .AddTypeExtension<TicketQuery>()

      .AddTypeExtension<UserQuery>()

      .AddProjections()
      .AddFiltering()
      .AddSorting()
      .AddDbContextCursorPagingProvider()
      .ModifyCostOptions(options =>
      {
        options.MaxFieldCost = 1_000_000;
        options.MaxTypeCost = 1_000_000;
        options.EnforceCostLimits = true;
        options.ApplyCostDefaults = true;
        options.DefaultResolverCost = 10.0;
      });
    return services;
  }
}