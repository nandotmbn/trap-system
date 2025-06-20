using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class ContentDelivery : Mandatory
	{
		[Key]
		[Required]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Permalink { get; set; } = string.Empty;
	}
}