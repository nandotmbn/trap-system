using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
	public class Substation : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required, MaxLength(20)]
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;

		public List<Camera> Cameras { get; set; } = [];
	}
}
