using Scalar.AspNetCore;
using Infrastructure.Services;
using WebAPI.Middlewares;
using WebAPI.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddOpenApi(options =>
{
	options.AddSchemaTransformer<JsonStringEnumSchemaTransformer>();
});
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

builder.Services.SignalRService(builder.Configuration);
builder.Services.ScopeService(builder.Configuration);
builder.Services.DatabaseServices(builder.Configuration);
builder.Services.AuthenticationService(builder.Configuration);
builder.Services.ControllerServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new EnumConverter()));

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference()
		.WithDisplayName("Boilerplate API");
}

app.MapControllers();
app.MapDefaultEndpoints();

app.UseStaticFiles();
string rootFolder = Path.Combine(builder.Environment.ContentRootPath, "CDN");
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(rootFolder),
	RequestPath = "/CDN"
});

app.UseMiddleware<TransactionRollbackMiddleware>();
app.Run();