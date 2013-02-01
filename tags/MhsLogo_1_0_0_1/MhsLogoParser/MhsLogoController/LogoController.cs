using System.Collections.Generic;
using MhsLogoParser;

namespace MhsLogoController
{
	public class LogoController
	{
		private static TurtleSituation currentSituation = TurtleSituation.DefaultSituation;

		private static readonly SymbolTable symbolTable = new SymbolTable();

		public static TurtleSituation CurrentSituation
		{
			get { return currentSituation; }
			set { currentSituation = value; }
		}

		public static ICollection<BaseLogoCommand> CreateAndParse(string programText)
		{
			var parser = new LogoParser(new LogoScanner(programText), symbolTable);
			return parser.ParseLogoProgram();
		}

		public static SymbolTableEntry LookupRoutine(string name)
		{
			return symbolTable.Lookup(name);
		}
	}
}