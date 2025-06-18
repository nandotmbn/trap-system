using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Models;

namespace Domain.Types
{
  public class CameraRequest
  {
    [Required, MaxLength(20)]
		public string Name { get; set; } = string.Empty;
		
		// For Camera
		public string? IP { get; set; } = string.Empty;
		public int Port { get; set; } = 554;
		public int Channel { get; set; } = 0;
		public int Subtype { get; set; } = 0;
		public string? Username { get; set; } = string.Empty;
		public string? Password { get; set; } = string.Empty;

		public Guid SubstationId { get; set; }
  }

  public record CameraResponse(HttpStatusCode StatusCode, string Message, Camera Data);
  public record CamerasResponse(HttpStatusCode StatusCode, string Message, List<Camera> Data);
}