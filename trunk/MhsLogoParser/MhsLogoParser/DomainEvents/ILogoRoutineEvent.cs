using System.Collections.Generic;
using MhsUtility;

namespace MhsLogoParser
{
	public interface ILogoRoutineEvent : IDomainEvent
	{
		IEnumerable<string> RoutineNames { get; }
	}
}