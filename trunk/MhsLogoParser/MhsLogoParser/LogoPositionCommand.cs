namespace MhsLogoParser
{
	public class LogoPositionCommand : ILogoCommand
	{
		public LogoPositionCommand(NumberRecord numberXRecord, NumberRecord numberYRecord)
		{
			X = numberXRecord.Number;
			Y = numberYRecord.Number;
		}

		public int X { get; set; }
		public int Y { get; set; }

		#region ILogoCommand Members

		public TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			var result = (TurtleSituation) currentSituation.Clone();
			result.Position = new Position {X = X, Y = Y};
			result.Change = TurtleSituationChange.Positioned;
			return result;
		}

		#endregion
	}
}