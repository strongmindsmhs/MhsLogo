using System.Collections.Generic;
using System.Collections.ObjectModel;
using MhsLogoParser.LogoCommands;
using MhsUtility;

namespace MhsLogoParser
{
	public class LogoDefineRoutineCommand : ILogoCommand
	{
		private readonly List<ILogoCommand> commands = new List<ILogoCommand>();

		public LogoDefineRoutineCommand(IdentifierRecord identifierRecord, IEnumerable<ILogoCommand> routineCommands)
		{
			Name = identifierRecord.Identifier;
			commands.AddRange(routineCommands);
		}

		public string Name { get; set; }

		public ReadOnlyCollection<ILogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}

		public override void Execute()
		{
			DomainEvents.Raise(new LogoCommandEvent(this));
		}
	}
}