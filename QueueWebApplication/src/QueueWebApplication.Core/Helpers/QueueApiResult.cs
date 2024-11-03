namespace QueueWebApplication.Core.Helpers;

public enum QueueApiResult
{
	AddedToQueue = 0,
	BypassedQueue = 1,
	Rejected = 3,
	AlreadyInQueue = 4
}
