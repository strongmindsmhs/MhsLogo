using System;
using MhsLogoParser;

namespace MhsLogoUI.Commands
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