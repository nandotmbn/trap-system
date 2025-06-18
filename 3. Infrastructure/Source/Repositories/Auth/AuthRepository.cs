using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repositories
{
  public class AuthRepository(IConfiguration configuration, AppDBContext appDBContext) : IAuth
  {
    async public Task<LoginResponse> Login(LoginRequest request)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var user = await appDBContext.Users
          .Where(e => e.Username == request.Credential || e.PhoneNumber == request.Credential)
          .Where(e => !e.IsArchived)
          .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found!");

        string token = JsonWebTokens.NewJsonWebTokens(user, configuration);

        return new LoginResponse(HttpStatusCode.OK, "Registered successfully", token);
      }
      catch
      {
        throw;
      }
    }

    async public Task<RegistrationResponse> Register(RegistrationRequest request)
    {
      using var transaction = appDBContext.Database.BeginTransaction();
      try
      {
        var isPhoneNumberExist = await appDBContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request!.PhoneNumber);
        if (isPhoneNumberExist != null) throw new BadRequestException("PhoneNumber is already registered!");
        var isUsernameExist = await appDBContext.Users.FirstOrDefaultAsync(u => u.Username == request!.Username);
        if (isUsernameExist != null) throw new BadRequestException("Username is already registered!");

        var user = new User
        {
          FirstName = request!.FirstName,
          LastName = request!.LastName,
          Username = request!.Username,
          PhoneNumber = request!.PhoneNumber,
          Password = BCrypt.Net.BCrypt.HashPassword(request!.Password),
        };

        appDBContext.Add(user);
        appDBContext.SaveChanges();
        transaction.Commit();

        return new RegistrationResponse(HttpStatusCode.Created, "Registered successfully", user);
      }
      catch
      {
        throw;
      }
    }
  }
}