using MhsUtility;

namespace MhsLogoParser.LogoCommands
{
	public interface ILogoCommandEvent : IDomainEvent
	{
		ILogoCommand LogoCommand { get; }
	}
}