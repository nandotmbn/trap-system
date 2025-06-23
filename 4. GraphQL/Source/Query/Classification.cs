using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class ClassificationQuery
{
  [UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<Classification> GetClassifications(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.Classifications.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Prediction.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countClassifications")]
  public async Task<int?> CountClassifications(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.Classifications.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Prediction.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<Classification?> GetClassificationAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Classifications.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}