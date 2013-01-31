using MhsUtility;

namespace MhsLogoParser.LogoCommands
{
	public interface ILogoCommandEvent : IDomainEvent
	{
		BaseLogoCommand LogoCommand { get; }
	}
}