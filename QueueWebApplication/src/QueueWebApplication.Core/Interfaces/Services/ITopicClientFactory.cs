using Byond.TopicSender;

namespace QueueWebApplication.Core.Interfaces.Services;

public interface ITopicClientFactory
{
	ITopicClient CreateTopicClient(TimeSpan timeout);
}
