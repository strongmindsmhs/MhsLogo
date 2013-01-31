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

		public int Repeat { get; private set; }

		public ReadOnlyCollection<ILogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}

		public override void Execute()
		{
			for (int i = 0; i < Repeat; i++)
			{
				foreach (ILogoCommand logoCommand in Commands)
				{
					logoCommand.Execute();
				}
			}
		}
	}
}