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
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<Detection> GetDetections(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.Detections.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Camera!.Name.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countDetections")]
  public async Task<int?> CountDetections(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.Detections.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Camera!.Name.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<Detection?> GetDetectionAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Detections.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}