using System;
using MhsLogoController;
using MhsLogoParser;
using MhsLogoUI.Commands;
using MhsLogoUI.ViewModel;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestMainWindowViewModel
	{
		#region Setup/Teardown

		[SetUp]
		public void SetUp()
		{
			sut = new MainWindowViewModel();
			ParseProgramCommand.Instance.ParseResult += OnProgramCommandParseResult;
		}

		[TearDown]
		public void TearDown()
		{
			ParseProgramCommand.Instance.Clear();
		}

		#endregion

		private MainWindowViewModel sut;

		private static void OnProgramCommandParseResult(object sender, ParseErrorEventArgs e)
		{
			if (e.Error)
			{
				throw new ApplicationException("Unexpected error: " + e.ErrorMessage);
			}
		}

		[Test, RequiresSTA]
		public void CanBackFromSkewedPosition()
		{
			int startX = LogoController.CurrentSituation.Position.X;
			int startY = LogoController.CurrentSituation.Position.Y;
			ParseProgramCommand.Instance.Execute("FORWARD 100 BACK 100");
			Assert.AreEqual(startX, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(startY, LogoController.CurrentSituation.Position.Y);
			ParseProgramCommand.Instance.Execute("RIGHT 180 FORWARD 100 BACK 100");
			Assert.AreEqual(startX, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(startY, LogoController.CurrentSituation.Position.Y);
		}

		[Test, RequiresSTA]
		public void CanHandleClearCommand()
		{
			ParseProgramCommand.Instance.Execute("FORWARD 100");
			Assert.AreEqual(2, sut.DrawingInstructions.Count);
			ParseProgramCommand.Instance.Execute("CLEAR");
			Assert.AreEqual(1, sut.DrawingInstructions.Count);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Position.X, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Position.Y, LogoController.CurrentSituation.Position.Y);
		}

		[Test, RequiresSTA]
		public void CanHandleMoveCommand()
		{
			ParseProgramCommand.Instance.Execute("MOVETO 100,100");
			Assert.AreEqual(1, sut.DrawingInstructions.Count);
			Assert.AreEqual(100, LogoController.CurrentSituation.Position.X);
			Assert.AreEqual(100, LogoController.CurrentSituation.Position.Y);
		}
	}
}