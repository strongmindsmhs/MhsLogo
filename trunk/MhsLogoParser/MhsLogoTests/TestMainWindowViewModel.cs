using MhsLogoController;
using MhsLogoUI.Commands;
using MhsLogoUI.ViewModel;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestMainWindowViewModel
	{
		[Test, RequiresSTA]
		public void CanBackFromSkewedPosition()
		{
			var programCommand = new ParseProgramCommand();
			var sut = new MainWindowViewModel(programCommand);
			int startX = LogoController.CurrentSituation.Position.X;
			int startY = LogoController.CurrentSituation.Position.Y;
			programCommand.Execute("FORWARD 100 BACK 100");
			Assert.AreEqual(startX, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(startY, LogoController.CurrentSituation.Position.Y);
			programCommand.Execute("RIGHT 180 FORWARD 100 BACK 100");
			Assert.AreEqual(startX, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(startY, LogoController.CurrentSituation.Position.Y);
		}
	}
}