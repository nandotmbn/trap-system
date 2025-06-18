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
	public IQueryable<Chat> GetChats(AppDBContext appDBContext)
	{
		var query = appDBContext.Chats.AsQueryable();
		return query;
	}

	[UseProjection]
	[QueryAuthorize]
	public async Task<Chat?> GetChatAsync(Guid id, AppDBContext appDBContext, CancellationToken cancellationToken)
	{
		return await appDBContext.Chats.FirstOrDefaultAsync(b => b.Id == id, cancellationToken)!;

	}
}