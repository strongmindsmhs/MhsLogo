using System.Windows.Input;
using MhsLogoUI.Commands;

namespace MhsLogoUI.ViewModel
{
	public class ConsoleViewModel : BaseViewModel
	{
		private string programCommandText;

		public ConsoleViewModel()
		{
			ParseProgramCommand.Instance.ParseResult += OnParseProgramCommandResult;
		}

		public string ProgramCommandText
		{
			get { return programCommandText; }
			set
			{
				programCommandText = value;
				OnPropertyChanged("ProgramCommandText");
			}
		}

		public static ICommand ConsoleProgramCommand
		{
			get { return ParseProgramCommand.Instance; }
		}

		private void OnParseProgramCommandResult(object sender, ParseErrorEventArgs e)
		{
			ProgramCommandText = e.ProgramCommand;
		}
	}
}