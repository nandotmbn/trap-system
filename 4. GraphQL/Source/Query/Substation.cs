using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class SubstationQuery
{
	[UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
	public IQueryable<Substation> GetSubstations(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Substations.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Name!.ToLower() + " " + x.Address!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Substation?> GetSubstationAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Substations.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}