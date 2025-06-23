using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class DetectionQuery
{
	[UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
	public IQueryable<Detection> GetDetections(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Detections.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Camera!.Name!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Detection?> GetDetectionAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Detections.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}