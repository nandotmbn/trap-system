using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class TicketQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Ticket> GetTickets(AppDBContext appDBContext)
	{
		var query = appDBContext.Tickets.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<Ticket?> GetTicketAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Tickets.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}