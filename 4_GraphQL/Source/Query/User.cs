using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class UserQuery
{
  [UseProjection]
  [UseSorting]
  [UseFiltering]
  [QueryAuthorize]
  [Pagination(DefaultLimit = 10, MaxLimit = 100)]
  public IQueryable<User> GetUsers(string? search, AppDBContext appDBContext)
  {
    var query = appDBContext.Users.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.FirstName!.ToLower() + " " + x.LastName!.ToLower(), $"%{search.ToLower()}%"));
    }

    return query;
  }

  [UseProjection]
  [UseFiltering]
  [QueryAuthorize]
  [GraphQLName("countUsers")]
  public async Task<int?> CountUsers(string? search, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    var query = appDBContext.Users.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.FirstName!.ToLower() + " " + x.LastName!.ToLower(), $"%{search.ToLower()}%"));
    }

    return await query.CountAsync(cancellationToken);
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<User?> GetUserAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Users.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}