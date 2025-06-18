using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Detection : Mandatory
	{
		[Key]
		[Required]
		public Guid Id { get; set; } = Guid.NewGuid();

		public Guid CameraId { get; set; }
		public Camera? Camera { get; set; }
		public List<Classification> Classifications { get; set; } = [];
		public List<Ticket> Tickets { get; set; } = [];
		public string Url { get; set; } = string.Empty;
	}
}