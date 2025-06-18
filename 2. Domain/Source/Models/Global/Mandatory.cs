using System.Text.Json.Serialization;

namespace Domain.Models
{
	public abstract class Mandatory
	{
		[JsonIgnore]
    public bool IsArchived {get; set;} = false;
		[JsonIgnore]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		[JsonIgnore]
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}

