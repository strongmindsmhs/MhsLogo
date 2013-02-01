namespace MhsUtility
{
	public interface IDomainEventHandler<T> where T : IDomainEvent
	{
		void Handle(T args);
	}
}