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
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<Ticket> GetTickets(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.Tickets.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Operator!.FirstName.ToLower() + " " + x.Operator!.LastName.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countTickets")]
  public async Task<int?> CountTickets(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.Tickets.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Operator!.FirstName.ToLower() + " " + x.Operator!.LastName.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<Ticket?> GetTicketAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Tickets.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}