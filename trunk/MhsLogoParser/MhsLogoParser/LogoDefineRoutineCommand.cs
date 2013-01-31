using System;

namespace MhsLogoParser
{
	public class LogoDefineRoutineCommand : ILogoCommand
	{
		public string Name { get; set; }

		public LogoDefineRoutineCommand(IdentifierRecord identifierRecord)
		{
			Name = identifierRecord.Identifier;
		}

		#region ILogoCommand Members

		public TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}