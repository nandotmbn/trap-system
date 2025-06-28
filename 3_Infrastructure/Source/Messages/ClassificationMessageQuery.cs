using System.Text;
using System.Text.Json;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messages;

public class ClassificationMessageQuery : BackgroundService
{
	private readonly ILogger<ClassificationMessageQuery> _logger;
	private readonly ConnectionFactory _factory;
	private IConnection? _connection;
	private IChannel? _channel;
	private readonly IHubContext<SignalRHub>? _hubContext;
	private readonly IServiceProvider _serviceProvider;
	private readonly string queueMessage;

	public ClassificationMessageQuery(ILogger<ClassificationMessageQuery> logger, IServiceProvider serviceProvider, IHubContext<SignalRHub> hubContext, IConfiguration configuration)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_hubContext = hubContext;
		_factory = new ConnectionFactory()
		{
			HostName = configuration["RabbitMQ:Host"]!,
			UserName = configuration["RabbitMQ:Username"]!,
			Password = configuration["RabbitMQ:Password"]!,
			Port = int.Parse(configuration["RabbitMQ:Port"] ?? "0"),
		};
		queueMessage = configuration["RabbitMQ:ClassifyQueue"]!;
	}

	async public Task<Guid> HandleMessage(ClassificationMessage message)
	{
		using var scope = _serviceProvider.CreateScope();
		using var appDBContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
		using var transaction = await appDBContext.Database.BeginTransactionAsync();
		try
		{
			var id = Guid.NewGuid();
			string fileName = $"{id}.jpg";
			if (message.Base64 != null)
			{
				byte[] imageBytes = Convert.FromBase64String(message.Base64.Split("base64,")[1]);
				using var stream = new FileStream($"CDN/{fileName}", FileMode.Create);
				await stream.WriteAsync(imageBytes);
			}

			var det = new Detection
			{
				Id = id,
				CameraId = message.StreamId,
				Classifications = [.. message.Classifications.Select(x => new Classification
				{
					Confidence = x.Confidence,
					Prediction = x.Prediction
				})],
			};

			appDBContext.Add(det);

			appDBContext.SaveChanges();
			transaction.Commit();

			return id;
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}

	public override async Task StartAsync(CancellationToken cancellationToken)
	{
		using var scope = _serviceProvider.CreateScope();
		_connection = await _factory.CreateConnectionAsync(cancellationToken);
		_channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);


		await _channel.QueueDeclareAsync(queue: queueMessage, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

		var consumer = new AsyncEventingBasicConsumer(_channel);
		consumer.ReceivedAsync += async (model, ea) =>
		{
			try
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				try
				{
					ClassificationMessage q = JsonSerializer.Deserialize<ClassificationMessage>(message)!;
					Guid res = await HandleMessage(q);

					await _hubContext?.Clients.All.SendAsync($"classify", res)!;
					await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

					_logger.LogInformation(" [✔] Message processed successfully.");
				}
				catch (Exception ex)
				{
					_logger.LogInformation("[x] Processing failed:" + ex.Message);
				}

			}
			catch (Exception ex)
			{
				_logger.LogError($"[x] Processing failed: {ex.Message}");
				await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
			}
		};

		await _channel.BasicConsumeAsync(queue: queueMessage, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		await _channel?.CloseAsync(cancellationToken: cancellationToken)!;
		await _connection?.CloseAsync(cancellationToken: cancellationToken)!;
	}

	public override void Dispose()
	{
		_channel?.Dispose();
		_connection?.Dispose();
		base.Dispose();
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
}
