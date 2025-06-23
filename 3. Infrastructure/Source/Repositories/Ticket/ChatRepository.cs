
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ChatRepository(AppDBContext appDBContext, IHttpContextAccessor accessor) : IChat
{
  async public Task<Chat> Create(ChatRequest request)
  {
    var chat = new Chat
    {
      Message = request.Message ?? "",
      Image = request.Image ?? "",
      CreatedById = UserClaim.GetId(accessor.HttpContext!.User)
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
    var userId = UserClaim.GetId(accessor.HttpContext!.User);
    var chat = await appDBContext.Chats
      .Where(e => e.Id == chatId)
      .Where(e => e.CreatedById == userId)
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