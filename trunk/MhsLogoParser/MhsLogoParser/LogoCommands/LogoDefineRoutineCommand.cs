using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class LogoDefineRoutineCommand : BaseLogoCommand
	{
		private readonly List<BaseLogoCommand> commands = new List<BaseLogoCommand>();

		public LogoDefineRoutineCommand(IdentifierRecord identifierRecord, IEnumerable<BaseLogoCommand> routineCommands)
		{
			Name = identifierRecord.Identifier;
			commands.AddRange(routineCommands);
		}

		public string Name { get; set; }

		public ReadOnlyCollection<BaseLogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}
	}
}