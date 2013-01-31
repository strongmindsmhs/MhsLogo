using System;
using MhsLogoParser.LogoCommands;
using MhsUtility;

namespace MhsLogoParser
{
	public class LogoTurnCommand : ILogoCommand
	{
		public LogoTurnCommand(NumberRecord numberRecord)
		{
			TurnAngle = numberRecord.Number;
		}

		public int TurnAngle { get; private set; }

		public override TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			var result = new TurtleSituation();
			int newAngle;
			if (TurnAngle >= 0)
			{
				newAngle = (currentSituation.Angle + TurnAngle)%360;
			}
			else
			{
				int turnAngleAbs = Math.Abs(TurnAngle);
				if (turnAngleAbs >= currentSituation.Angle)
				{
					newAngle = 360 - (turnAngleAbs - currentSituation.Angle);
				}
				else
				{
					newAngle = currentSituation.Angle - turnAngleAbs;
				}
			}
			result.Angle = newAngle;
			result.TurnAngle = currentSituation.TurnAngle + TurnAngle;
			result.Position = currentSituation.Position;
			result.Change = TurtleSituationChange.Turned;
			return result;
		}

		public override void Execute()
		{
			DomainEvents.Raise(new LogoCommandEvent(this));
		}
	}
}