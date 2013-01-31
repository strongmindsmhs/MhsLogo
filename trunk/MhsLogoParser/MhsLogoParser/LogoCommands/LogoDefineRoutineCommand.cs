using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class LogoDefineRoutineCommand : ILogoCommand
	{
		public string Name { get; set; }

		private readonly List<ILogoCommand> commands = new List<ILogoCommand>();

		public ReadOnlyCollection<ILogoCommand> Commands
		{
			get 
			{
				return commands.AsReadOnly();
			}
		}

		public LogoDefineRoutineCommand(IdentifierRecord identifierRecord, IEnumerable<ILogoCommand> routineCommands)
		{
			Name = identifierRecord.Identifier;
			commands.AddRange(routineCommands);
		}

		#region ILogoCommand Members

		public TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}