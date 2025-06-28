using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<UserAttributes>))]
public enum UserAttributes
{
  FirstName,
  LastName,
  Username,
  PhoneNumber,
  CreatedAt,
  UpdatedAt
};

public interface IUser
{
  IQueryable<User> Query(string? search, string? name, string? username, string? phoneNumber, UserType? type);

  Task<UserResponse> GetUser(Guid userId);
  Task<UsersResponse> GetUsers(string? search, string? name, string? username, string? phoneNumber, UserType? type, SortType sortType = SortType.Asc, UserAttributes sortAttribute = UserAttributes.FirstName, int page = 1, int limit = 10);
  Task<CountResponse> CountUsers(string? search, string? name, string? username, string? phoneNumber, UserType? type);

  Task<User> Recovery(Guid userId, RecoveryRequest request);
  Task<User> Update(Guid userId, UserRequest request);
  Task<User> Delete(Guid userId);
  Task<UserResponse> RecoveryUser(Guid userId, RecoveryRequest request);
  Task<UserResponse> UpdateUser(Guid userId, UserRequest request);
  Task<UserResponse> DeleteUser(Guid userId);
}