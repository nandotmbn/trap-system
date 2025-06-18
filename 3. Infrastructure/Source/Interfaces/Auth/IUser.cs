
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface IUser
  {
    Task<User> Recovery(Guid userId, RecoveryRequest request);
    Task<User> Update(Guid userId, UserRequest request);
    Task<User> Delete(Guid userId);
    Task<UserResponse> RecoveryUser(Guid userId, RecoveryRequest request);
    Task<UserResponse> UpdateUser(Guid userId, UserRequest request);
    Task<UserResponse> DeleteUser(Guid userId);
  }
}