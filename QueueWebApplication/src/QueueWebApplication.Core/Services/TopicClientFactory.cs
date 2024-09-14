using Byond.TopicSender;
using Microsoft.Extensions.Logging;
using QueueWebApplication.Core.Interfaces.Services;

namespace QueueWebApplication.Core.Services;

public sealed class TopicClientFactory : ITopicClientFactory
{
	private readonly ILogger<TopicClient>? _logger;

	public TopicClientFactory(ILogger<TopicClient> logger)
	{
		ArgumentNullException.ThrowIfNull(logger);

		if (logger.IsEnabled(LogLevel.Trace))
			_logger = logger;
	}

	public ITopicClient CreateTopicClient(TimeSpan timeout)
		=> new TopicClient(
			new SocketParameters
			{
				ConnectTimeout = timeout,
				DisconnectTimeout = timeout,
				ReceiveTimeout = timeout,
				SendTimeout = timeout,
			},
			_logger);
}
