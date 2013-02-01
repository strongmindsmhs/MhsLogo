using MhsUtility;

namespace MhsLogoParser
{
	public interface ILogoCommandEvent : IDomainEvent
	{
		BaseLogoCommand LogoCommand { get; }
	}
}