using Domain.Models;
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
	public IQueryable<Camera> GetCameras(AppDBContext appDBContext)
  {
    var query = appDBContext.Cameras.AsQueryable();
		return query;
	}

	[UseProjection]
	public async Task<Camera?> GetCameraAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Cameras.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
		
	}
}