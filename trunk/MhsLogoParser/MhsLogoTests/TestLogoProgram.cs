using System.Collections.Generic;
using System.Linq;
using MhsLogoController;
using MhsLogoParser;
using MhsLogoParser.LogoCommands;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestLogoProgram
	{
		[Test]
		public void CanCreateLogoRoutine()
		{
			LogoController.CreateAndParse("TO RECTANGLE REPEAT 4 [ FORWARD 100 LEFT 90 ] END");
			SymbolTableEntry routine = LogoController.LookupRoutine("RECTANGLE");
			Assert.IsNotNull(routine);
			Assert.AreEqual("RECTANGLE", routine.Name);
			var routineAttribute = routine.Attributes[0] as SymbolTableRoutineAttribute;
			Assert.IsNotNull(routineAttribute);
			var firstCommand = routineAttribute.Commands[0] as LogoRepeatCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(4, firstCommand.Repeat);
			Assert.AreEqual(2, firstCommand.Commands.Count());
		}

		[Test]
		public void CanCreateRepeatLogoProgram()
		{
			ICollection<BaseLogoCommand> programCommands = LogoController.CreateAndParse("REPEAT 4 [ FORWARD 100 LEFT 90 ]");
			var firstCommand = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstCommand);
			Assert.AreEqual(4, firstCommand.Repeat);
			Assert.AreEqual(2, firstCommand.Commands.Count);
		}

		[Test]
		public void CanCreateTwoSentenceLogoProgram()
		{
			ICollection<BaseLogoCommand> programCommands = LogoController.CreateAndParse("FORWARD 100 LEFT 90");
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
			ICollection<BaseLogoCommand> programCommands =
				LogoController.CreateAndParse("REPEAT 3 [ FORWARD 1 REPEAT 2 [ FORWARD 1 ] ]");
			var firstRepeat = programCommands.ElementAt(0) as LogoRepeatCommand;
			Assert.IsNotNull(firstRepeat);
			Assert.AreEqual(3, firstRepeat.Repeat);
			var secondCommand = firstRepeat.Commands.ElementAt(0) as LogoMoveCommand;
			Assert.IsNotNull(secondCommand);
			var secondRepeat = firstRepeat.Commands.ElementAt(1) as LogoRepeatCommand;
			Assert.IsNotNull(secondRepeat);
			Assert.AreEqual(2, secondRepeat.Repeat);
		}

		[Test]
		public void CanUseLogoRoutine()
		{
			ICollection<BaseLogoCommand> programCommands =
				LogoController.CreateAndParse("TO RECTANGLE REPEAT 4 [ FORWARD 100 LEFT 90 ] END RECTANGLE");
			var routineCallCommand = programCommands.ElementAt(0) as LogoRoutineCallCommand;
			Assert.IsNotNull(routineCallCommand);
		}
	}
}