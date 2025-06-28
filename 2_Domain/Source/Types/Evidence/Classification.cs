using System.Net;
using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain.Types
{
	public class ClassificationResultMessage
	{
		[JsonPropertyName("prediction")]
		public string Prediction { get; set; } = string.Empty;
		[JsonPropertyName("confidence")]
		public double Confidence { get; set; } = 0.0;
	}

	public class ClassificationMessage
	{
		[JsonPropertyName("stream_id")]
		public Guid StreamId { get; set; }
		[JsonPropertyName("timestamp")]
		public DateTime Timestamp { get; set; }
		[JsonPropertyName("areas")]
		public List<double[]> Areas { get; set; } = [];
		[JsonPropertyName("classifications")]
		public List<ClassificationResultMessage> Classifications { get; set; } = [];
		[JsonPropertyName("base64")]
		public string Base64 { get; set; } = string.Empty;
	}
	
  public record ClassificationResponse(HttpStatusCode StatusCode, string Message, Classification Data);
  public record ClassificationsResponse(HttpStatusCode StatusCode, string Message, List<Classification> Data);
}