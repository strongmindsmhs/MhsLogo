using System;
using System.Windows;
using System.Windows.Media;
using MhsLogoParser;

namespace MhsLogoUI.ViewModel
{
	public class TurtleInstruction : BaseInstruction
	{
		private double centerX;
		private double centerY;
		private PathGeometry movement;
		private PointCollection points;
		private TimeSpan timeOffset;
		private double turnAngle;

		public TurtleInstruction(TurtleSituation currentSituation)
		{
			ToPoints(currentSituation);
		}

		public PointCollection Points
		{
			get { return points; }
			private set
			{
				points = value;
				OnPropertyChanged("Points");
			}
		}

		public PathGeometry Movement
		{
			get { return movement; }
			set
			{
				movement = value;
				OnPropertyChanged("Movement");
			}
		}

		public TimeSpan TimeOffset
		{
			get { return timeOffset; }
			set
			{
				timeOffset = value;
				OnPropertyChanged("TimeOffset");
			}
		}

		public double TurnAngle
		{
			get { return turnAngle; }
			set
			{
				turnAngle = value;
				OnPropertyChanged("TurnAngle");
			}
		}

		public double CenterX
		{
			get { return centerX; }
			set
			{
				centerX = value;
				OnPropertyChanged("CenterX");
			}
		}

		public double CenterY
		{
			get { return centerY; }
			set
			{
				centerY = value;
				OnPropertyChanged("CenterY");
			}
		}

		public void ToPoints(TurtleSituation situation)
		{
			Points = new PointCollection
			         	{
			         		new Point(situation.Position.X + 10, situation.Position.Y),
			         		new Point(situation.Position.X, situation.Position.Y + 20),
			         		new Point(situation.Position.X - 10, situation.Position.Y)
			         	};
		}
	}
}