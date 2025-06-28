using System.Net;
using Domain.Models;

namespace Domain.Types
{
	public class TicketRequest
	{
		public TicketStatus Status { get; set; }
		public Guid DetectionId { get; set; }
	}
	public class TicketStatusRequest
	{
		public TicketStatus Status { get; set; }
	}

	public record TicketResponse(HttpStatusCode StatusCode, string Message, Ticket Data);
	public record TicketsResponse(HttpStatusCode StatusCode, string Message, List<Ticket> Data);
}