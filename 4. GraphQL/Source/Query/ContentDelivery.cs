using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class ContentDeliveryQuery
{
  [UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<ContentDelivery> GetContentDeliveries(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.ContentDeliveries.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Title.ToLower() + " " + x.Permalink.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countContentDeliveries")]
  public async Task<int?> CountContentDeliveries(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.ContentDeliveries.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Title.ToLower() + " " + x.Permalink.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<ContentDelivery?> GetContentDeliveryAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.ContentDeliveries.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}