namespace MhsLogoParser
{
	public class LogoClearCommand : ILogoCommand
	{
		#region ILogoCommand Members

		public TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			TurtleSituation.DefaultTurtleSituation result = TurtleSituation.DefaultSituation;
			result.Change = TurtleSituationChange.Cleared;
			return result;
		}

		#endregion
	}
}