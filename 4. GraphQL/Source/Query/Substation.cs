using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class SubstationQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Substation> GetSubstations(AppDBContext appDBContext)
	{
		var query = appDBContext.Substations.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<Substation?> GetSubstationAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Substations.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}