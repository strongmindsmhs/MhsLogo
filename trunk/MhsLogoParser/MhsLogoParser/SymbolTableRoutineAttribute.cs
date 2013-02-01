using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class SymbolTableRoutineAttribute : SymbolTableAttribute
	{
		private readonly List<BaseLogoCommand> commands = new List<BaseLogoCommand>();

		public SymbolTableRoutineAttribute(IEnumerable<BaseLogoCommand> routineCommands)
		{
			commands.AddRange(routineCommands);
		}

		public ReadOnlyCollection<BaseLogoCommand> Commands
		{
			get { return commands.AsReadOnly(); }
		}

	}
}