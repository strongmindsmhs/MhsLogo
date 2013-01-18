using System.Collections.Generic;
using System.Linq;
using MhsLogoController;
using MhsLogoParser;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestLogoProgram
	{
		[Test]
		public void CanCreateRepeatLogoProgram()
		{
			var directionForwardCmd = new LogoMoveCommand(new NumberRecord(Token.FORWARD, "100"));
			var directionLeftCmd = new LogoTurnCommand(new NumberRecord(Token.LEFT, "90"));
			var repeatCmd = new LogoRepeatCommand(new NumberRecord(Token.REPEAT, "4"),
			                                      new List<ILogoCommand> {directionForwardCmd, directionLeftCmd});
			ICollection<ILogoCommand> programCommands = LogoController.CreateAndParse("REPEAT 4 [ FORWARD 100 LEFT 90 ]");
			var firstCommand = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(repeatCmd.Repeat, firstCommand.Repeat);
			Assert.AreEqual(repeatCmd.Commands.Count, firstCommand.Commands.Count);
		}

		[Test]
		public void CanCreateTwoSentenceLogoProgram()
		{
			var directionForwardCmd = new LogoMoveCommand(new NumberRecord(Token.FORWARD, "100"));
			var directionLeftCmd = new LogoTurnCommand(new NumberRecord(Token.LEFT, "90"));
			ICollection<ILogoCommand> programCommands = LogoController.CreateAndParse("FORWARD 100 LEFT 90");
			var firstCommand = programCommands.ElementAt(0) as LogoMoveCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(100, directionForwardCmd.Distance);
			TurtleSituation forwardSituation = directionForwardCmd.CalculateSituation(TurtleSituation.DefaultSituation);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Angle, forwardSituation.Angle);
			var secondCommand = programCommands.ElementAt(1) as LogoTurnCommand;
			Assert.IsNotNull(secondCommand);
			Assert.AreEqual(90, directionLeftCmd.TurnAngle);
			TurtleSituation leftSituation = directionLeftCmd.CalculateSituation(forwardSituation);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Angle + 90, leftSituation.Angle);
		}

		[Test]
		public void CanRepeatRepeatLogoProgram()
		{
			var directionForwardCmd = new LogoMoveCommand(new NumberRecord(Token.FORWARD, "1"));
			var innerRepeatCmd = new LogoRepeatCommand(new NumberRecord(Token.REPEAT, "2"),
			                                           new List<ILogoCommand> {directionForwardCmd});
			var outerRepeatCmd = new LogoRepeatCommand(new NumberRecord(Token.REPEAT, "3"),
			                                           new List<ILogoCommand> {innerRepeatCmd, directionForwardCmd});
			ICollection<ILogoCommand> programCommands =
				LogoController.CreateAndParse("REPEAT 3 [ FORWARD 1 REPEAT 2 [ FORWARD 1 ] ]");
			var firstRepeat = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstRepeat);
			Assert.AreEqual(outerRepeatCmd.Repeat, firstRepeat.Repeat);
			var secondCommand = firstRepeat.Commands.ElementAt(0) as LogoMoveCommand;
			Assert.IsNotNull(secondCommand);
			var secondRepeat = firstRepeat.Commands.ElementAt(1) as LogoRepeatCommand;
			Assert.IsNotNull(secondRepeat);
			Assert.AreEqual(innerRepeatCmd.Repeat, secondRepeat.Repeat);
		}
	}
}