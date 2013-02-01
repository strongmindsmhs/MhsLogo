using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class LogoRepeatCommand : BaseLogoCommand
	{
		private readonly List<BaseLogoCommand> commands = new List<BaseLogoCommand>();

		public LogoRepeatCommand(NumberRecord repeatNumber, IEnumerable<BaseLogoCommand> logoCommands)
		{
			Repeat = repeatNumber.Number;
			commands.AddRange(logoCommands);
		}

		public int Repeat { get; private set; }

		public ReadOnlyCollection<BaseLogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}

		public override void Execute()
		{
			for (int i = 0; i < Repeat; i++)
			{
				foreach (BaseLogoCommand logoCommand in Commands)
				{
					logoCommand.Execute();
				}
			}
		}
	}
}