using Infrastructure.Services;
using GraphQL.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddCors(options =>
  {
    options.AddDefaultPolicy(builder =>
    {
      builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
  });
builder.Services.DatabaseServices(builder.Configuration);
builder.Services.AuthenticationService(builder.Configuration);
builder.Services.QueryService(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapGraphQL();
app.MapDefaultEndpoints();


app.Run();
