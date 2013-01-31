using System;
using System.Collections.Generic;
using System.Windows.Input;
using MhsLogoController;
using MhsLogoParser;

namespace MhsLogoUI.Commands
{
	public class ParseProgramCommand : ICommand
	{
		#region Singleton implementation

		private static volatile ParseProgramCommand instance;
		private static readonly object syncRoot = new Object();

		private ParseProgramCommand()
		{
		}

		public static ParseProgramCommand Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new ParseProgramCommand();
					}
				}

				return instance;
			}
		}

		public void Clear()
		{
			instance = null;
		}

		#endregion

		#region ICommand Members

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			var program = (string) parameter;
			try
			{
				ICollection<ILogoCommand> commands = LogoController.CreateAndParse(program);
				foreach (ILogoCommand logoCommand in commands)
				{
					logoCommand.Execute();
				}
				FireParseResult(new ParseErrorEventArgs(false));
			}
			catch (Exception ex)
			{
				if (ex is LogoScannerException || ex is LogoSyntaxErrorException)
				{
					FireParseResult(new ParseErrorEventArgs(true, ex.Message, program));
				}
				else
				{
					throw;
				}
			}
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		#endregion

		public event EventHandler<ParseErrorEventArgs> ParseResult;

		public void FireParseResult(ParseErrorEventArgs e)
		{
			EventHandler<ParseErrorEventArgs> handler = ParseResult;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}