using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;
using MhsLogoController;
using MhsLogoParser;
using MhsLogoUI.Commands;

namespace MhsLogoUI.ViewModel
{
	public class MainWindowViewModel : BaseViewModel
	{
		private readonly Polygon turtleShape;
		private ObservableCollection<Shape> drawingInstructions = new ObservableCollection<Shape>();

		private string parseError;

		public MainWindowViewModel(ParseProgramCommand parseProgramCommand)
		{
			parseProgramCommand.LogoCommand += OnLogoCommand;
			parseProgramCommand.ParseResult += OnParseProgramCommandResult;
			turtleShape = new Polygon();
			turtleShape.ToTurtle(LogoController.CurrentSituation);
			DrawingInstructions.Add(turtleShape);
		}

		public MainWindowViewModel(): 
			this(ParseProgramCommand.Instance)
		{
		}

		public string ParseError
		{
			get { return parseError; }
			private set
			{
				parseError = value;
				OnPropertyChanged("ParseError");
			}
		}

		public ObservableCollection<Shape> DrawingInstructions
		{
			get { return drawingInstructions; }
			set
			{
				drawingInstructions = value;
				OnPropertyChanged("DrawingInstructions");
			}
		}

		private void OnParseProgramCommandResult(object sender, ParseErrorEventArgs e)
		{
			ParseError = e.ErrorMessage;
		}

		private void OnLogoCommand(object sender, LogoCommandEventArgs e)
		{
			TurtleSituation currentSituation = LogoController.CurrentSituation;
			TurtleSituation newSituation = e.LogoCommand.CalculateSituation(currentSituation);

			switch (newSituation.Change)
			{
				case TurtleSituationChange.Moved:
					var brush = new SolidColorBrush(Colors.Black);
					DrawingInstructions.Add(new Line
					                        	{
					                        		X1 = currentSituation.Position.X,
					                        		Y1 = currentSituation.Position.Y,
					                        		X2 = newSituation.Position.X,
					                        		Y2 = newSituation.Position.Y,
					                        		Stroke = brush,
					                        		StrokeThickness = 3
					                        	});
					break;

				case TurtleSituationChange.Cleared:
					DrawingInstructions.Clear();
					break;

				case TurtleSituationChange.Positioned:
				case TurtleSituationChange.None:
				case TurtleSituationChange.Turned:
					// Do nothing
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			DoTurtle(newSituation);
			LogoController.CurrentSituation = newSituation;
		}

		private void DoTurtle(TurtleSituation newSituation)
		{
			var rotateTransform = new RotateTransform(newSituation.TurnAngle)
			                      	{
			                      		CenterX = newSituation.Position.X,
			                      		CenterY = newSituation.Position.Y
			                      	};
			turtleShape.ToTurtle(newSituation);
			turtleShape.RenderTransform = rotateTransform;
			drawingInstructions.Remove(turtleShape);
			drawingInstructions.Add(turtleShape);
		}
	}
}