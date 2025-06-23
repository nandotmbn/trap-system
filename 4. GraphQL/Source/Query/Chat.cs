using Domain.Models;
using GraphQL.Middlewares;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Source;

[ExtendObjectType(typeof(Query))]
public class ChatQuery
{
	[UsePaging(IncludeTotalCount = true)]
	[UseProjection]
	[UseFiltering]
	[UseSorting]
	[QueryAuthorize]
	public IQueryable<Chat> GetChats(string? search, AppDBContext appDBContext, int page = 1, int limit = int.MaxValue)
  {
    int? itemsToSkip = (page - 1) * limit;
    
    var query = appDBContext.Chats.AsQueryable();
    if (search != null && search != "")
    {
      query = query.Where(x => EF.Functions.Like(x.Message!.ToLower() + " " + x.Image!.ToLower(), $"%{search.ToLower()}%"));
    }

    query = query.Skip((int)itemsToSkip!).Take(limit);

    return query;
  }

	[UseProjection]
	[QueryAuthorize]
	public async Task<Chat?> GetChatAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Chats.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}