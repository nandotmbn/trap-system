using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class CameraQuery
{
  [UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<Camera> GetCameras(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.Cameras.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Name.ToLower() + " " + x.Substation!.Name.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countCameras")]
  public async Task<int?> CountCameras(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.Cameras.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<Camera?> GetCameraAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Cameras.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}