using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Chat : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		
		public string Message { get; set; } = string.Empty;
		public string Image { get; set; } = string.Empty;

		public Guid CreatedById { get; set; }
		public User? CreatedBy { get; set; }

		public Guid TicketId { get; set; }
		public Ticket? Ticket { get; set; }
	}
}
