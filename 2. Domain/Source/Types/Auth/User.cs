using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Models;

namespace Domain.Types
{
  public class ChangePasswordRequest
  {
    [Required]
    public string? CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64)]
    public string? NewPassword { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64), Compare(nameof(NewPassword))]
    public string? NewPasswordConfirmation { get; set; } = string.Empty;
  }

  public class RecoveryRequest
  {
    [Required, MinLength(8), MaxLength(64)]
    public string Password { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(64), Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; } = string.Empty;
  }

  public class SelfRequest
  {
    [Required, MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string LastName { get; set; } = string.Empty;
    [Required, MaxLength(64)]
    public string Username { get; set; } = string.Empty;
    [PhoneOrEmpty]
		public string PhoneNumber { get; set; } = string.Empty;
  }

  public class UserRequest : SelfRequest
  {
    public UserType Type { get; set; } = UserType.OPERATOR;
  }

  public record UserResponse(HttpStatusCode StatusCode, string Message, User Data);
  public record UsersResponse(HttpStatusCode StatusCode, string Message, List<User> Data);
}