using System;

namespace MhsLogoParser
{
	public class LogoMoveCommand : BaseLogoCommand
	{
		public LogoMoveCommand(NumberRecord numberRecord)
		{
			Distance = numberRecord.Number;
		}

		public int Distance { get; private set; }

		public override TurtleSituation CalculateSituation(TurtleSituation currentSituation)
		{
			var result = new TurtleSituation();
			int newX = Convert.ToInt32(Math.Round(Distance*Math.Cos(DegreesToRadian(currentSituation.Angle))));
			int newY = Convert.ToInt32(Math.Round(Math.Sqrt(Distance*Distance - newX*newX)));
			result.Position = new Position
			                  	{
			                  		X = currentSituation.Position.X + newX,
			                  		Y = CalculateY(currentSituation, newY)
			                  	};
			result.Angle = currentSituation.Angle;
			result.TurnAngle = currentSituation.TurnAngle;
			result.Change = TurtleSituationChange.Moved;
			return result;
		}

		private int CalculateY(TurtleSituation currentSituation, int newY)
		{
			int result;
			if ((currentSituation.Angle <= 180 && Math.Sign(Distance) > 0) ||
			    (currentSituation.Angle > 180 && Math.Sign(Distance) < 0))
			{
				result = currentSituation.Position.Y + newY;
			}
			else
			{
				result = currentSituation.Position.Y - newY;
			}
			return result;
		}

		private static double DegreesToRadian(int angle)
		{
			return (Math.PI/180)*angle;
		}
	}
}