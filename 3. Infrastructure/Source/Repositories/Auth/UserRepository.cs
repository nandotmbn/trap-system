
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository(AppDBContext appDBContext) : IUser
{
  async public Task<User> Delete(Guid userId)
  {
    var user = await appDBContext.Users
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pengguna tidak ditemukan");

    await appDBContext.Users.ExecuteUpdateAsync(s => s.SetProperty(p => p.IsArchived, true));

    return user;
  }

  async public Task<UserResponse> DeleteUser(Guid userId)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var user = await Delete(userId);
      transaction.Commit();

      return new UserResponse(HttpStatusCode.Accepted, "Pengguna berhasil dihapus", user);
    }
    catch
    {
      throw;
    }
  }

  async public Task<User> Recovery(Guid userId, RecoveryRequest request)
  {
    var user = await appDBContext.Users
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pengguna tidak ditemukan");

    user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

    appDBContext.Update(user);
    appDBContext.SaveChanges();

    return user;
  }

  async public Task<UserResponse> RecoveryUser(Guid userId, RecoveryRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var user = await Recovery(userId, request);
      transaction.Commit();

      return new UserResponse(HttpStatusCode.Accepted, "Pengguna berhasil diperbaiki", user);
    }
    catch
    {
      throw;
    }
  }

  async public Task<User> Update(Guid userId, UserRequest request)
  {
    var user = await appDBContext.Users
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pengguna tidak ditemukan");

    user.FirstName = request.FirstName;
    user.LastName = request.LastName;
    user.Username = request.Username;
    user.PhoneNumber = request.PhoneNumber;
    user.Type = request.Type;

    appDBContext.Update(user);
    appDBContext.SaveChanges();

    return user;
  }

  async public Task<UserResponse> UpdateUser(Guid userId, UserRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var user = await Update(userId, request);
      transaction.Commit();

      return new UserResponse(HttpStatusCode.Accepted, "Pengguna berhasil dihapus", user);
    }
    catch
    {
      throw;
    }
  }
}