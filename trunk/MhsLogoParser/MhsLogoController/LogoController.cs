using System.Collections.Generic;
using MhsLogoParser;

namespace MhsLogoController
{
	public class LogoController
	{
		private static TurtleSituation currentSituation = TurtleSituation.DefaultSituation;

		public static TurtleSituation CurrentSituation
		{
			get { return currentSituation; }
			set { currentSituation = value; }
		}

		public static ICollection<ILogoCommand> CreateAndParse(string programText)
		{
			var parser = new LogoParser(new LogoScanner(programText));
			return parser.ParseLogoProgram();
		}
	}
}