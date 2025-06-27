
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<ChatAttributes>))]
public enum ChatAttributes
{
  Message,
  CreatedAt,
  UpdatedAt
};

public interface IChat
{
  IQueryable<Chat> Query(string? search, string? message, Guid? ticketId);
  Task<ChatResponse> GetChat(Guid chatId);
  Task<ChatsResponse> GetChats(string? search, string? message, Guid? ticketId, SortType sortType = SortType.Asc, ChatAttributes sortAttribute = ChatAttributes.CreatedAt, int page = 1, int limit = 10);
  Task<CountResponse> CountChats(string? search, string? message, Guid? ticketId);
  
  Task<Chat> Create(ChatRequest request);
  Task<Chat> Update(Guid chatId, ChatRequest request);
  Task<Chat> Delete(Guid chatId);
  Task<ChatResponse> CreateChat(ChatRequest request);
  Task<ChatResponse> UpdateChat(Guid chatId, ChatRequest request);
  Task<ChatResponse> DeleteChat(Guid chatId);
}
