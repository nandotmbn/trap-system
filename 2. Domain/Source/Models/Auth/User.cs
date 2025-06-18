using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models
{
	public enum UserType {
		Admin,
		Operator,
		Supervisor,
		Manager,
	}

	public class User : Mandatory
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required, MaxLength(20)]
		public string? FirstName { get; set; } = string.Empty;
		[Required, MaxLength(20)]
		public string? LastName { get; set; } = string.Empty;
		[Required, MaxLength(64)]
		public string? Username { get; set; } = string.Empty;
		[Required, MaxLength(64), MinLength(8)]
		[JsonIgnore]
		public string? Password { get; set; } = string.Empty;
		[PhoneOrEmpty]
		public string? PhoneNumber { get; set; } = string.Empty;
		public UserType Type { get; set; } = UserType.Operator;
		
		public List<Ticket> Tickets { get; set; } = [];
		public List<Chat> Chats { get; set; } = [];
	}
}

