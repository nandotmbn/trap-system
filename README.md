# .NET 9 Database Sharding Using Entity Framework and PostgreSQL with Simple Authentication

Building scalable web applications requires a robust strategy for handling growing datasets. One popular technique is **database sharding** — splitting your data horizontally across multiple database instances to distribute load and improve performance.

In this article, we will explore how to implement **database sharding** in a **.NET 9** application using **Entity Framework Core** and **PostgreSQL**. Additionally, we'll integrate a simple but secure **JWT-based authentication** system that leverages sharding information to route requests correctly.

---

## Table of Contents

- [.NET 9 Database Sharding Using Entity Framework and PostgreSQL with Simple Authentication](#net-9-database-sharding-using-entity-framework-and-postgresql-with-simple-authentication)
  - [Table of Contents](#table-of-contents)
  - [Why Sharding?](#why-sharding)
  - [Project Overview](#project-overview)
  - [Configuration: Defining Shards](#configuration-defining-shards)
  - [Shard Selection Logic](#shard-selection-logic)
  - [Shard-Aware Entity Framework Context](#shard-aware-entity-framework-context)
  - [User Models and Shard Metadata](#user-models-and-shard-metadata)
  - [User Registration Flow with Sharding](#user-registration-flow-with-sharding)
  - [Login and JWT Authentication](#login-and-jwt-authentication)
  - [JWT Claims for Shard Routing](#jwt-claims-for-shard-routing)
  - [Putting It All Together: Program Setup](#putting-it-all-together-program-setup)
  - [Summary and Next Steps](#summary-and-next-steps)
- [Appendix: Code Snippets and Helper Classes](#appendix-code-snippets-and-helper-classes)

---

## Why Sharding?

A single database can become a bottleneck as your user base and data grow:

- Queries become slower under heavy load
- Database backups and maintenance take longer
- Scaling vertically is costly and limited

**Sharding** horizontally partitions your data across multiple databases (shards), improving:

- Performance by distributing reads/writes
- Availability by isolating faults
- Scalability by adding more shards as needed

---

## Project Overview

Our goal is to build a .NET 9 API that:

- Supports multiple PostgreSQL shards, each with its own connection string
- Uses a central shard registry (shard 0) to map users to shards
- Dynamically selects the correct database shard per user request
- Employs JWT authentication with shard info embedded in the token claims

---

## Configuration: Defining Shards

The shard connection strings are configured in `appsettings.json`. Each shard runs on a different port for this example:

```json
{
  "ConnectionStrings": {
    "Default": "Server=host.docker.internal;PORT=4810;Database=database;Username=devuser;Password=devpassword;"
  },
  "Shards": {
    "0": "Server=host.docker.internal;PORT=4810;Database=database;Username=devuser;Password=devpassword;",
    "1": "Server=host.docker.internal;PORT=4811;Database=database;Username=devuser;Password=devpassword;",
    "2": "Server=host.docker.internal;PORT=4812;Database=database;Username=devuser;Password=devpassword;",
    "3": "Server=host.docker.internal;PORT=4813;Database=database;Username=devuser;Password=devpassword;",
    "4": "Server=host.docker.internal;PORT=4814;Database=database;Username=devuser;Password=devpassword;"
  },
  "Jwt": {
    "Key": "your-super-secret-key",
    "Issuer": "http://localhost:8080/",
    "Audience": "http://localhost:8080/"
  }
}
```

## Shard Selection Logic

We implement a service to select the current shard based on the authenticated user's claims:

```cs
public interface IShardSelector
{
    int GetCurrentShard();
}

public class ShardSelector : IShardSelector
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ShardSelector(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetCurrentShard()
    {
        int index = 0;
        try
        {
            return UserClaim.GetIndex(_httpContextAccessor.HttpContext?.User!);
        }
        catch
        {
            // Default to shard 0 if claim is missing or invalid
        }

        return index;
    }
}
```

`UserClaim.GetIndex` extracts the shard index from the JWT claims.

## Shard-Aware Entity Framework Context

Our EF Core DbContext dynamically uses the connection string of the correct shard:

```cs
public static IServiceCollection DatabaseServices(this IServiceCollection services, IConfiguration configuration)
{
    List<IConfigurationSection> shards = configuration.GetSection("Shards").GetChildren().ToList();

    var shardConfig = new ShardConfiguration();
    var defaultShard = configuration["ConnectionStrings:Default"];
    shardConfig.DefaultConnection = defaultShard!;

    for (int i = 0; i < shards.Count; i++)
    {
        if (shards[i].Value != null)
            shardConfig.Shards!.Add(i, shards[i].Value!);
    }

    if (shardConfig.Shards == null || shardConfig.Shards.Count == 0)
        throw new InvalidOperationException("Shards configuration is missing or empty.");

    services.AddSingleton(shardConfig);
    services.AddScoped<IShardSelector, ShardSelector>();

    services.AddDbContext<AppDBContext>((serviceProvider, options) =>
    {
        var shard = serviceProvider.GetRequiredService<IShardSelector>().GetCurrentShard();
        options.UseNpgsql(shardConfig.Shards[shard], b => b.MigrationsAssembly("WebAPI"));
    }, ServiceLifetime.Scoped);

    return services;
}
```

This means EF Core will connect to the correct PostgreSQL shard per request.

## User Models and Shard Metadata

We keep minimal shard metadata in shard 0, and full user data in the user shard:

```cs
public class UserShard
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [Required, Range(0, int.MaxValue)]
    public int Shard { get; set; } = 0;
}

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(20)]
    public string? FirstName { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string? LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(64), MinLength(5)]
    public string? Email { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(64)]
    public string? Username { get; set; } = string.Empty;

    [JsonIgnore, MaxLength(64), MinLength(8)]
    public string? Password { get; set; } = string.Empty;
}
```

## User Registration Flow with Sharding

When registering a new user:

1. Generate a GUID for the user.
2. Calculate shard index from the GUID:
   
   ```cs
    public static int GetShardIndex(Guid id, IConfiguration configuration)
    {
        int shardCount = configuration.GetSection("Shards").GetChildren().Count();
        return (int)(BitConverter.ToUInt32(id.ToByteArray(), 0) % shardCount);
    }
   ```

3. Save a `UserShard` record to shard 0 to map `user` → `shard`.
4. Save full user data to the appropriate shard.
   
   Here’s the registration method:
   
   ```cs
    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
      var userId = Guid.NewGuid();
      var index = AppDBContextFactory.GetShardIndex(userId, configuration);

      using var defaultDbContext = AppDBContextFactory.CreateDbContext(0, configuration);
      using var defaultTransaction = await defaultDbContext.Database.BeginTransactionAsync();

      // Check for duplicate email/username in shard 0
      var isEmailExist = await defaultDbContext.UserShards.FirstOrDefaultAsync(u => u.Email == request.Email);
      if (isEmailExist != null) throw new BadRequestException("Email is already registered!");

      var isUsernameExist = await defaultDbContext.UserShards.FirstOrDefaultAsync(u => u.Username == request.Username);
      if (isUsernameExist != null) throw new BadRequestException("Username is already registered!");

      defaultDbContext.Add(new UserShard
      {
          Id = userId,
          Shard = index,
          Email = request.Email,
          Username = request.Username
      });

      await defaultDbContext.SaveChangesAsync();
      await defaultTransaction.CommitAsync();

      using var appDBContext = AppDBContextFactory.CreateDbContext(index, configuration);
      using var transaction = await appDBContext.Database.BeginTransactionAsync();

      var user = new User
      {
          Id = userId,
          FirstName = request.FirstName,
          LastName = request.LastName,
          Username = request.Username,
          Email = request.Email,
          Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
      };

      appDBContext.Add(user);
      await appDBContext.SaveChangesAsync();
      await transaction.CommitAsync();

      return new RegistrationResponse(HttpStatusCode.Created, "Registered successfully", user);
    }
   ```

## Login and JWT Authentication

Login flow:

1. Look up the user in shard 0 to get the shard index.
2. Query the user details from the respective shard.
3. Generate a JWT embedding the shard index.

```cs
public async Task<LoginResponse> Login(LoginRequest request)
{
  int userShard = 0;

  using var defaultDbContext = AppDBContextFactory.CreateDbContext(0, configuration);
  var userShardEntry = await defaultDbContext.UserShards
      .Where(e => e.Username == request.Credential || e.Email == request.Credential)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found!");

  userShard = userShardEntry.Shard;

  using var appDBContext = AppDBContextFactory.CreateDbContext(userShard, configuration);
  var user = await appDBContext.Users
      .Where(e => e.Username == request.Credential || e.Email == request.Credential)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("User not found!");

  string token = JsonWebTokens.NewJsonWebTokens(user, userShard, configuration);

  return new LoginResponse(HttpStatusCode.Created, "Logged in successfully", token);
}

```

## JWT Claims for Shard Routing
Our tokens embed the shard index as a claim:

```cs
public static class UserClaim
{
    public static int GetIndex(ClaimsPrincipal claimsPrincipal)
    {
        string identifierClaim = claimsPrincipal.FindFirst(ClaimTypes.Version)!.Value;
        return int.TryParse(identifierClaim, out int index)
            ? index
            : throw new InternalServerErrorException("Shard index claim is invalid!");
    }
}

```

## Putting It All Together: Program Setup

In `Program.cs` or `Startup.cs`, add:

```cs
builder.Services.AddHttpContextAccessor();
builder.Services.DatabaseServices(builder.Configuration);
```

Make sure your app uses authentication middleware:

```cs
app.UseAuthentication();
app.UseAuthorization();
```

## Summary and Next Steps

We implemented a shard-aware authentication system with:

- A central shard index to track users’ shard assignment
- Deterministic shard allocation based on user ID
- EF Core context that dynamically connects to the correct PostgreSQL shard
- JWT tokens embedding shard info for automatic routing

Next steps:

- Set up EF migrations for all shards
- Add logging and better error handling
- Extend the user model and authentication flows
- Build a frontend that consumes these APIs securely

Feel free to reach out if you want the full source code or help extending this system!

# Appendix: Code Snippets and Helper Classes

1. `UserClaim.cs Helper`

```cs
public static class UserClaim
{
    public static Guid GetId(ClaimsPrincipal claimsPrincipal)
    {
        string id = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        return Guid.TryParse(id, out Guid userId) ? userId
            : throw new InternalServerErrorException("User Id is invalid!");
    }

    public static int GetIndex(ClaimsPrincipal claimsPrincipal)
    {
        string index = claimsPrincipal.FindFirst(ClaimTypes.Version)!.Value;
        return int.TryParse(index, out int shardIndex)
            ? shardIndex
            : throw new InternalServerErrorException("Shard index is invalid!");
    }
}

```

2. `AppDBContextFactory.cs Helper`

```cs
public static class AppDBContextFactory
{
    public static AppDBContext CreateDbContext(int shardIndex, IConfiguration configuration)
    {
        var shards = configuration.GetSection("Shards").GetChildren();
        var shardConfig = shards.FirstOrDefault(s => s.Key == shardIndex.ToString());

        if (shardConfig == null)
            throw new Exception("Shard connection string not found!");

        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseNpgsql(shardConfig.Value, b => b.MigrationsAssembly("WebAPI"));

        return new AppDBContext(optionsBuilder.Options);
    }

    public static int GetShardIndex(Guid id, IConfiguration configuration)
    {
        int shardCount = configuration.GetSection("Shards").GetChildren().Count();
        return (int)(BitConverter.ToUInt32(id.ToByteArray(), 0) % shardCount);
    }
}

```