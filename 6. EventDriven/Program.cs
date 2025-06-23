using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.AddServiceDefaults();

builder.Services.SignalRService(builder.Configuration);
builder.Services.DatabaseServices(builder.Configuration);
builder.Services.ConsumerService(builder.Configuration);

var app = builder.Build();

app.MapHub<SignalRHub>("api/signalr");
app.MapDefaultEndpoints();
app.Run();