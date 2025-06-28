using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Domain.Types
{
  public class LoginRequest
  {
    [Required, MaxLength(64)]
    public string? Credential { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64)]
    public string? Password { get; set; } = string.Empty;
  }
	public record LoginResponse(HttpStatusCode StatusCode, string Message, string Token);
}