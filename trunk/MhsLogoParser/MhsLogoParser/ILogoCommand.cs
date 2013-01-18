namespace MhsLogoParser
{
	public interface ILogoCommand
	{
		TurtleSituation CalculateSituation(TurtleSituation currentSituation);
	}
}