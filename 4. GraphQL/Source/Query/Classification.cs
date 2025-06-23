using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class ClassificationQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Classification> GetClassifications(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Classifications.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Prediction!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Classification?> GetClassificationAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Classifications.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}