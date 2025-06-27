using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter<TicketStatus>))]
	public enum TicketStatus
	{
		PENDING,
		SAFE,
		STANDBY,
		CRITICAL
	}

	public class Ticket : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		public bool IsOpen { get; set; } = true;
		public string TicketNumber { get; set; } = string.Empty;
		public TicketStatus Status { get; set; } = TicketStatus.PENDING;
		public string GeneratedReportUrl { get; set; } = string.Empty;

		public Guid OperatorId { get; set; }
		public User? Operator { get; set; }

		public Guid DetectionId { get; set; }
		public Detection? Detection { get; set; }

		public List<Chat> Chats { get; set; } = [];
	}
}
