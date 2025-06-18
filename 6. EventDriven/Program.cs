var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();
app.Run();