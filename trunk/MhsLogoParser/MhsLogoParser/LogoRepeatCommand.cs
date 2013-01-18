using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class LogoRepeatCommand : ILogoCommand
	{
		private readonly List<ILogoCommand> commands = new List<ILogoCommand>();

		public LogoRepeatCommand(NumberRecord repeatNumber, IEnumerable<ILogoCommand> logoCommands)
		{
			Repeat = repeatNumber.Number;
			commands.AddRange(logoCommands);
		}

		public ReadOnlyCollection<ILogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}

		public int Repeat { get; private set; }

		#region ILogoCommand Members

		public TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			return currentSituation;
		}

		#endregion
	}
}