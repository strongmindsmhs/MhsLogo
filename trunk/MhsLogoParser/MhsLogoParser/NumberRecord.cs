using System;

namespace MhsLogoParser
{
	public class NumberRecord
	{
		public int Number { get; private set; }

		public NumberRecord(Token directionToken, string numberAsString)
		{
			int convertedNumber;
			if (!int.TryParse(numberAsString, out convertedNumber))
			{
				throw new LogoSyntaxErrorException(String.Format("Unsupported number '{0}'", numberAsString), LogoErrorCode.NumberError, numberAsString);
			}
			Number = convertedNumber;
			if (directionToken == Token.BACK || directionToken == Token.RIGHT)
			{
				Number = (-1)*Number;
			}
		}
	}
}