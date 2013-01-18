namespace MhsLogoParser
{
	public class NumberRecord
	{
		public int Number { get; private set; }

		public NumberRecord(Token directionToken, string numberAsString)
		{
			Number = int.Parse(numberAsString);
			if (directionToken == Token.BACK || directionToken == Token.RIGHT)
			{
				Number = (-1)*Number;
			}
		}
	}
}