namespace MhsLogoParser
{
	public abstract class BaseLogoCommand
	{
		public virtual void Execute()
		{
		}

		public virtual TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			return currentSituation;
		}
	}
}