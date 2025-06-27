
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
  public IQueryable<User> Query(string? search, string? name, string? username, string? phoneNumber, UserType? type)
  {
    var query = appDBContext.Users.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.FirstName.ToLower() + " " +
        x.LastName.ToLower() + " " +
        x.PhoneNumber.ToLower() + " " +
        x.Username.ToLower(),
        $"%{search.ToLower()}%"));
    }
    if (name != null)
    {
      query = query.Where(x => EF.Functions.Like(x.FirstName.ToLower() + " " + x.LastName.ToLower(), $"%{name.ToLower()}%"));
    }
    if (username != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Username.ToLower(), $"%{username.ToLower()}%"));
    }
    if (phoneNumber != null)
    {
      query = query.Where(x => EF.Functions.Like(x.PhoneNumber.ToLower(), $"%{phoneNumber.ToLower()}%"));
    }
    if (type != null)
    {
      query = query.Where(x => x.Type == type);
    }

    return query;
  }

  async public Task<UserResponse> GetUser(Guid userId)
  {
    var user = await appDBContext.Users
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Pengguna tidak ditemukan");

    return new UserResponse(HttpStatusCode.OK, "Pengguna berhasil didapatkan", user);
  }
  async public Task<UsersResponse> GetUsers(string? search, string? name, string? username, string? phoneNumber, UserType? type, SortType sortType = SortType.Asc, UserAttributes sortAttribute = UserAttributes.FirstName, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, name, username, phoneNumber, type);

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, UserAttributes.FirstName) => query.OrderBy(q => q.FirstName),
      (SortType.Asc, UserAttributes.LastName) => query.OrderBy(q => q.LastName),
      (SortType.Asc, UserAttributes.PhoneNumber) => query.OrderBy(q => q.PhoneNumber),
      (SortType.Asc, UserAttributes.Username) => query.OrderBy(q => q.Username),
      (SortType.Asc, UserAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, UserAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, UserAttributes.FirstName) => query.OrderByDescending(q => q.FirstName),
      (SortType.Desc, UserAttributes.LastName) => query.OrderByDescending(q => q.LastName),
      (SortType.Desc, UserAttributes.PhoneNumber) => query.OrderByDescending(q => q.PhoneNumber),
      (SortType.Desc, UserAttributes.Username) => query.OrderByDescending(q => q.Username),
      (SortType.Desc, UserAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, UserAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new UsersResponse(HttpStatusCode.OK, "Pengguna berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountUsers(string? search, string? name, string? username, string? phoneNumber, UserType? type)
  {
    var query = Query(search, name, username, phoneNumber, type);
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

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

      return new UserResponse(HttpStatusCode.Accepted, "Pengguna berhasil diubah", user);
    }
    catch
    {
      throw;
    }
  }
}