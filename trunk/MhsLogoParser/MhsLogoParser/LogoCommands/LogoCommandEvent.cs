namespace MhsLogoParser.LogoCommands
{
	public class LogoCommandEvent : ILogoCommandEvent
	{
		private readonly BaseLogoCommand command;

		public LogoCommandEvent(BaseLogoCommand logoCommand)
		{
			command = logoCommand;
		}

		#region ILogoCommandEvent Members

		public BaseLogoCommand LogoCommand
		{
			get { return command; }
		}

		#endregion
	}
}