
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface IChat
  {
    Task<Chat> Create(ChatRequest request);
    Task<Chat> Update(Guid chatId, ChatRequest request);
    Task<Chat> Delete(Guid chatId);
    Task<ChatResponse> CreateChat(ChatRequest request);
    Task<ChatResponse> UpdateChat(Guid chatId, ChatRequest request);
    Task<ChatResponse> DeleteChat(Guid chatId);
  }
}