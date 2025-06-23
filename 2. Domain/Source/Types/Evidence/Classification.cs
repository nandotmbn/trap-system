using System.Text.Json.Serialization;

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
}