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
		public void CanCreateLogoRoutine()
		{
			ICollection<ILogoCommand> programCommands =
				LogoController.CreateAndParse("TO RECTANGLE REPEAT 4 [ FORWARD 100 LEFT 90 ] END");
			var routine = programCommands.ElementAt(0) as LogoDefineRoutineCommand;
			Assert.IsNotNull(routine);
			Assert.AreEqual("RECTANGLE", routine.Name);
			var firstCommand = routine.Commands[0] as LogoRepeatCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(4, firstCommand.Repeat);
			Assert.AreEqual(2, firstCommand.Commands.Count());
		}

		[Test]
		public void CanCreateRepeatLogoProgram()
		{
			ICollection<ILogoCommand> programCommands = LogoController.CreateAndParse("REPEAT 4 [ FORWARD 100 LEFT 90 ]");
			var firstCommand = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(4, firstCommand.Repeat);
			Assert.AreEqual(2, firstCommand.Commands.Count);
		}

		[Test]
		public void CanCreateTwoSentenceLogoProgram()
		{
			ICollection<ILogoCommand> programCommands = LogoController.CreateAndParse("FORWARD 100 LEFT 90");
			var firstCommand = programCommands.ElementAt(0) as LogoMoveCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(100, firstCommand.Distance);
			TurtleSituation forwardSituation = firstCommand.CalculateSituation(TurtleSituation.DefaultSituation);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Angle, forwardSituation.Angle);
			var secondCommand = programCommands.ElementAt(1) as LogoTurnCommand;
			Assert.IsNotNull(secondCommand);
			Assert.AreEqual(90, secondCommand.TurnAngle);
			TurtleSituation leftSituation = secondCommand.CalculateSituation(forwardSituation);
			Assert.AreEqual(TurtleSituation.DefaultSituation.Angle + 90, leftSituation.Angle);
		}

		[Test]
		public void CanRepeatRepeatLogoProgram()
		{
			ICollection<ILogoCommand> programCommands = LogoController.CreateAndParse("REPEAT 3 [ FORWARD 1 REPEAT 2 [ FORWARD 1 ] ]");
			var firstRepeat = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstRepeat);
			Assert.AreEqual(3, firstRepeat.Repeat);
			var secondCommand = firstRepeat.Commands.ElementAt(0) as LogoMoveCommand;
			Assert.IsNotNull(secondCommand);
			var secondRepeat = firstRepeat.Commands.ElementAt(1) as LogoRepeatCommand;
			Assert.IsNotNull(secondRepeat);
			Assert.AreEqual(2, secondRepeat.Repeat);
		}
	}
}