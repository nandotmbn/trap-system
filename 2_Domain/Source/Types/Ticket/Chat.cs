using System.Net;
using Domain.Models;

namespace Domain.Types
{
	public class ChatRequest
	{
		public string? Message { get; set; } = string.Empty;
		public string? Image { get; set; } = string.Empty;
		public Guid TicketId { get; set; }
	}

	public record ChatResponse(HttpStatusCode StatusCode, string Message, Chat Data);
	public record ChatsResponse(HttpStatusCode StatusCode, string Message, List<Chat> Data);
}