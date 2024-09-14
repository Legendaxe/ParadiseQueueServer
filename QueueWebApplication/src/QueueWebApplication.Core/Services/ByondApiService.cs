using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Byond.TopicSender;
using Microsoft.Extensions.Logging;
using QueueWebApplication.Core.DTOs;
using QueueWebApplication.Core.Entities;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class ByondApiService(ILogger<ByondApiService> logger, ITopicClientFactory topicClientFactory) : IByondApiService
{
	private readonly ITopicClient _byondTopicSender = topicClientFactory.
		CreateTopicClient(TimeSpan.FromMilliseconds(5000));


	private async ValueTask<TopicResponse?> SendTopicRequest(string commandString,
		Server server, CancellationToken cancellationToken = default)
	{
		try
		{
			var byondResponse = await _byondTopicSender.SendTopic(
				server.IpAddress,
				commandString,
				server.Port,
				cancellationToken);
			return byondResponse;
		}
		catch (Exception e) when (e is not OperationCanceledException)
		{
			logger.LogWarning("SendTopic exception! {Ip}:{Port}", server.IpAddress, server.Port);
		}

		return null;
	}


	public async ValueTask<bool> IsClientAllowedToServer(WaitingClientDto clientDto, Server server, CancellationToken cancellationToken = default)
	{
		var response = await SendTopicRequest($"queuecheck&ckey={clientDto.Ckey}", server, cancellationToken);

		if (response is not null) return false;
		logger.Log(LogLevel.Warning, "{Ip}:{Port} did not respond on QueueCheck topic", server.IpAddress,
			server.Port);

		return false;

	}

	public async ValueTask<int> AvailableSlotsOnServer(Server server, CancellationToken cancellationToken = default)
	{
		var response = await SendTopicRequest("ping", server, cancellationToken);

		if (response?.FloatData is null)
		{
			return 0;
		}

		return server.MaximumPlayers - (int) response.FloatData;
	}

	public async ValueTask<IEnumerable<string>> GetPlayersList(Server server,
		CancellationToken cancellationToken = default)
	{
		var response = await SendTopicRequest("playerlist", server, cancellationToken);
		if (response?.StringData is null)
		{
			return [];
		}
		return JsonSerializer.Deserialize<IEnumerable<string>>(response.StringData) ?? [];
	}
}
