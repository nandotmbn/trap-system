using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models
{
	public class Camera : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required, MaxLength(20)]
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		
		// For Camera
		public string? IP { get; set; } = string.Empty;
		public int Port { get; set; } = 554;
		public int Channel { get; set; } = 0;
		public int Subtype { get; set; } = 0;
		public string? Username { get; set; } = string.Empty;
		public string? Password { get; set; } = string.Empty;

		[JsonIgnore]
		public List<Detection> Detections { get; set; } = [];
	}
}
