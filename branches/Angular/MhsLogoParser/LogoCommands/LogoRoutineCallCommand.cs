using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser.LogoCommands
{
	public class LogoRoutineCallCommand : BaseLogoCommand
	{
		private readonly List<BaseLogoCommand> routineCommands = new List<BaseLogoCommand>();

		public LogoRoutineCallCommand(string routineName, IEnumerable<BaseLogoCommand> commands)
		{
			RoutineName = routineName;
			routineCommands.AddRange(commands);
		}

		public string RoutineName { get; set; }

		public ReadOnlyCollection<BaseLogoCommand> Commands
		{
			get { return routineCommands.AsReadOnly(); }
		}

		public override void Execute()
		{
			foreach (BaseLogoCommand logoCommand in Commands)
			{
				logoCommand.Execute();
			}
		}
	}
}