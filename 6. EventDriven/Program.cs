using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.DatabaseServices(builder.Configuration);
builder.Services.ConsumerService(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();
app.Run();