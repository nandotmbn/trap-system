using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
  public class UserEntityBuilder()
  {
    public static void Set(ModelBuilder modelBuilder, IConfiguration configuration)
    {
      var password = BCrypt.Net.BCrypt.HashPassword(configuration["SuperUser:Password"]);
      string id = configuration["SuperUser:Id"]!;

      if (Guid.TryParse(id, out Guid userId))
      {
        modelBuilder.Entity<User>().HasData(
          new User
          {
            Id = userId,
            FirstName = "PLN Security",
            LastName = "Super Admin",
            Username = configuration["SuperUser:Username"]!,
            Password = password,
            Type = UserType.ADMIN,
          }
        );
      }
    }
  }
}