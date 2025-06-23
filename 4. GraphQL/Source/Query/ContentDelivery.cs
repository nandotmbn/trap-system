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
	public IQueryable<ContentDelivery> GetContentDeliveries(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.ContentDeliveries.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Title!.ToLower() + " " + x.Permalink!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<ContentDelivery?> GetContentDeliveryAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.ContentDeliveries.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}