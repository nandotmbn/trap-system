using System.Text.Json.Serialization;

namespace Domain.Types
{
	public class ClassificationMessage
	{
		[JsonPropertyName("stream_id")]
		public Guid StreamId { get; set; }
		[JsonPropertyName("timestamp")]
		public DateTime Timestamp { get; set; }
		[JsonPropertyName("areas")]
		public List<double[]> Areas { get; set; } = [];
		[JsonPropertyName("base64")]
		public string Base64 { get; set; } = string.Empty;
  }
}