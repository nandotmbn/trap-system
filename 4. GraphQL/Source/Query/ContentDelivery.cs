using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class ContentDeliveryQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<ContentDelivery> GetContentDeliveries(AppDBContext appDBContext)
	{
		var query = appDBContext.ContentDeliveries.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<ContentDelivery?> GetContentDeliveryAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.ContentDeliveries.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}