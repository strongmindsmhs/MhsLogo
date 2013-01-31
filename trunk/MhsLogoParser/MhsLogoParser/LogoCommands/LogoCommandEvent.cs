namespace MhsLogoParser.LogoCommands
{
	public class LogoCommandEvent : ILogoCommandEvent
	{
		private readonly ILogoCommand command;

		public LogoCommandEvent(ILogoCommand logoCommand)
		{
			command = logoCommand;
		}

		#region ILogoCommandEvent Members

		public ILogoCommand LogoCommand
		{
			get { return command; }
		}

		#endregion
	}
}