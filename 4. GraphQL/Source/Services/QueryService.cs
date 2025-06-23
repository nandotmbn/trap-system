
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
      })
      .ConfigureSchema(builder =>
      {
        builder.Use(next => async context =>
        {
          try
          {
            await next(context);

            // Apply pagination after ALL other middleware
            if (context.Selection.Field.ContextData.ContainsKey("pagination.enabled") &&
                  context.Result is IQueryable queryable)
            {
              var page = context.ArgumentValue<int?>("page") ?? 1;
              var limit = context.ArgumentValue<int?>("limit");

              var defaultLimit = (int)context.Selection.Field.ContextData["pagination.defaultLimit"]!;
              var maxLimit = (int)context.Selection.Field.ContextData["pagination.maxLimit"]!;

              var validatedLimit = Math.Min(Math.Max(limit ?? defaultLimit, 1), maxLimit);
              var validatedPage = Math.Max(page, 1);
              var skip = (validatedPage - 1) * validatedLimit;

              var elementType = queryable.ElementType;

              var skipMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Skip" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(elementType);

              var takeMethod = typeof(Queryable).GetMethods()
                    .First(m => m.Name == "Take" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(elementType);

              var skippedQuery = skipMethod.Invoke(null, new object[] { queryable, skip });
              var paginatedQuery = takeMethod.Invoke(null, new object[] { skippedQuery!, validatedLimit });

              context.Result = paginatedQuery;
            }
          }
          catch
          {
          }
        });
      });
    return services;
  }
}