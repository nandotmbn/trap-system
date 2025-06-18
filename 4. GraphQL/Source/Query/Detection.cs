using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class DetectionQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Detection> GetDetections(AppDBContext appDBContext)
	{
		var query = appDBContext.Detections.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<Detection?> GetDetectionAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Detections.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}