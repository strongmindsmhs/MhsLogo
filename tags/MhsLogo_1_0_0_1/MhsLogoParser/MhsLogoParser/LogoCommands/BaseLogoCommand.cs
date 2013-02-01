using MhsUtility;

namespace MhsLogoParser
{
	public abstract class BaseLogoCommand
	{
		public virtual void Execute()
		{
			DomainEvents.Raise(new LogoCommandEvent(this));
		}

		public virtual TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			return currentSituation;
		}
	}
}