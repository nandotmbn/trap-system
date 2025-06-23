using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class CameraQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Camera> GetCameras(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Cameras.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Name!.ToLower() + " " + x.Substation!.Name!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Camera?> GetCameraAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Cameras.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}