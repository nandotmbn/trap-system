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
	public IQueryable<Classification> GetClassifications(AppDBContext appDBContext)
	{
		var query = appDBContext.Classifications.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<Classification?> GetClassificationAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Classifications.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}