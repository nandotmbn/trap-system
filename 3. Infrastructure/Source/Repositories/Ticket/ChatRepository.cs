
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChatRepository(AppDBContext appDBContext, IHttpContextAccessor accessor, IHubContext<SignalRHub> hubContext) : IChat
{
  public IQueryable<Chat> Query(string? search, string? message, Guid? ticketId)
  {
    var query = appDBContext.Chats.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.Message.ToLower() + " ",
        $"%{search.ToLower()}%"));
    }
    if (message != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Message.ToLower(), $"%{message.ToLower()}%"));
    }
    if (ticketId != null)
    {
      query = query.Where(x => x.TicketId == ticketId);
    }

    return query;
  }

  async public Task<ChatResponse> GetChat(Guid chatId)
  {
    var user = await appDBContext.Chats
      .Where(e => e.Id == chatId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pesan tidak ditemukan");

    return new ChatResponse(HttpStatusCode.OK, "Pesan berhasil didapatkan", user);
  }
  async public Task<ChatsResponse> GetChats(string? search, string? message, Guid? ticketId, SortType sortType = SortType.Asc, ChatAttributes sortAttribute = ChatAttributes.CreatedAt, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, message, ticketId); ;

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, ChatAttributes.Message) => query.OrderBy(q => q.Message),
      (SortType.Asc, ChatAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, ChatAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, ChatAttributes.Message) => query.OrderByDescending(q => q.Message),
      (SortType.Desc, ChatAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, ChatAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new ChatsResponse(HttpStatusCode.OK, "Pesan berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountChats(string? search, string? message, Guid? ticketId)
  {
    var query = Query(search, message, ticketId); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

  async public Task<Chat> Create(ChatRequest request)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == request.TicketId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Ticket tidak ditemukan");

    if (!ticket.IsOpen) throw new BadRequestException("Ticket sudah ditutup");

    var chat = new Chat
    {
      Message = request.Message ?? "",
      Image = request.Image ?? "",
      CreatedById = UserClaim.GetId(accessor.HttpContext!.User),
      TicketId = request.TicketId
    };
    await appDBContext.AddAsync(chat);
    await appDBContext.SaveChangesAsync();

    return chat;
  }

  async public Task<ChatResponse> CreateChat(ChatRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var chat = await Create(request);
      await hubContext?.Clients.All.SendAsync("ticket-" + request.TicketId, "push")!;

      transaction.Commit();
      return new ChatResponse(HttpStatusCode.Created, "Pesan berhasil dibuat", chat);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Chat> Delete(Guid chatId)
  {
    var chat = await appDBContext.Chats
      .Where(e => e.Id == chatId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pesan tidak ditemukan");

    await appDBContext.Chats.ExecuteUpdateAsync(s => s.SetProperty(p => p.IsArchived, true));

    return chat;
  }

  async public Task<ChatResponse> DeleteChat(Guid chatId)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var chat = await Delete(chatId);
      transaction.Commit();

      return new ChatResponse(HttpStatusCode.Accepted, "Pesan berhasil dihapus", chat);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Chat> Update(Guid chatId, ChatRequest request)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == request.TicketId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Ticket tidak ditemukan");

    if (!ticket.IsOpen) throw new BadRequestException("Ticket sudah ditutup");

    var userId = UserClaim.GetId(accessor.HttpContext!.User);
    var chat = await appDBContext.Chats
      .Where(e => e.Id == chatId)
      .Where(e => e.CreatedById == chatId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pesan tidak ditemukan");

    chat.Message = request.Message ?? chat.Message;
    chat.Image = request.Image ?? chat.Image;

    appDBContext.Update(chat);
    appDBContext.SaveChanges();

    return chat;
  }

  async public Task<ChatResponse> UpdateChat(Guid chatId, ChatRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var chat = await Update(chatId, request);
      transaction.Commit();

      return new ChatResponse(HttpStatusCode.Accepted, "Pesan berhasil diubah", chat);
    }
    catch
    {
      throw;
    }
  }
}