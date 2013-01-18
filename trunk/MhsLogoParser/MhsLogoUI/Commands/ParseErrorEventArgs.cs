using System;

namespace MhsLogoUI.Commands
{
	public class ParseErrorEventArgs : EventArgs
	{
		public ParseErrorEventArgs(bool error, string errorMessage, string programCommand)
		{
			Error = error;
			ErrorMessage = errorMessage;
			ProgramCommand = programCommand;
		}

		public bool Error { get; private set; }
		public string ErrorMessage { get; private set; }
		public string ProgramCommand { get; private set; }
	}
}