using System.Collections.Generic;

namespace MhsLogoParser
{
	public class LogoRoutineEvent : ILogoRoutineEvent
	{
		private readonly IEnumerable<string> routineNames;

		public LogoRoutineEvent(IEnumerable<string> routineNames)
		{
			this.routineNames = routineNames;
		}

		#region ILogoRoutineEvent Members

		public IEnumerable<string> RoutineNames
		{
			get { return routineNames; }
		}

		#endregion
	}
}