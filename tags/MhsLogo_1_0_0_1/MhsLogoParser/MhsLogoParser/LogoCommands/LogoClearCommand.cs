namespace MhsLogoParser
{
	public class LogoClearCommand : BaseLogoCommand
	{
		public override TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			TurtleSituation.DefaultTurtleSituation result = TurtleSituation.DefaultSituation;
			result.Change = TurtleSituationChange.Cleared;
			return result;
		}
	}
}