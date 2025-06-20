using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class UserQuery
{
  [UsePaging(IncludeTotalCount = true)]
  [UseProjection]
  [UseFiltering]
  [UseSorting]
  [QueryAuthorize]
  public IQueryable<User> GetUsers(AppDBContext appDBContext)
  {
    var query = appDBContext.Users.AsQueryable();
    return query;
  }

  [UseProjection]
  [QueryAuthorize]
  public async Task<User?> GetUserAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
  {
    return await appDBContext.Users.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;
  }
}