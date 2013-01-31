using System;

namespace MhsLogoParser.LogoCommands
{
	public class LogoCommandEventArgs : EventArgs
	{
		public LogoCommandEventArgs(ILogoCommand logoCommand)
		{
			LogoCommand = logoCommand;
		}

		public ILogoCommand LogoCommand { get; private set; }
	}
}