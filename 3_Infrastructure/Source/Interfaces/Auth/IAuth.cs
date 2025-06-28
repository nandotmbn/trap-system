using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface IAuth
  {
    Task<LoginResponse> Login(LoginRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
  }
}