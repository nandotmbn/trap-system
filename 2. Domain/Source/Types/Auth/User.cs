using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Models;

namespace Domain.Types
{
  
  public class ChangePasswordUserRequest
  {
    [Required]
    public string? CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64)]
    public string? NewPassword { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64), Compare(nameof(NewPassword))]
    public string? NewPasswordConfirmation { get; set; } = string.Empty;
  }

  public record UserResponse(HttpStatusCode StatusCode, string Message, User Data);
  public record UsersResponse(HttpStatusCode StatusCode, string Message, List<User> Data);
}