using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class TicketQuery
{
	[UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
	public IQueryable<Ticket> GetTickets(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Tickets.AsQueryable();
    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Ticket?> GetTicketAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Tickets.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}