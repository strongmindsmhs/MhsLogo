using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using MhsLogoParser;

namespace MhsLogoUI.ViewModel
{
	public static class PolygonExtensions
	{
		public static Polygon ToTurtle(this Polygon turtle, TurtleSituation situation)
		{
			var brush = new SolidColorBrush(Colors.Blue);
			turtle.Points = new PointCollection
			                	{
			                		new Point(situation.Position.X + 10, situation.Position.Y),
			                		new Point(situation.Position.X, situation.Position.Y + 20),
			                		new Point(situation.Position.X - 10, situation.Position.Y)
			                	};
			turtle.Stroke = brush;
			turtle.StrokeThickness = 3;
			return turtle;
		}
	}
}