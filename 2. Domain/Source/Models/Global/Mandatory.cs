using System.Text.Json.Serialization;

namespace Domain.Models
{
	public abstract class Mandatory
	{
		[JsonIgnore]
    public bool IsArchived {get; set;} = false;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}

