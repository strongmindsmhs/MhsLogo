namespace MhsLogoParser
{
	public abstract class ILogoCommand
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