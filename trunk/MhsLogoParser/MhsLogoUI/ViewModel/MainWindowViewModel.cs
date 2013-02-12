using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MhsLogoController;
using MhsLogoParser;
using MhsLogoUI.Commands;
using MhsUtility;

namespace MhsLogoUI.ViewModel
{
	public class MainWindowViewModel : BaseViewModel, IDomainEventHandler<ILogoCommandEvent>
	{
		private const int TURTLE_SPEED_IN_MS = 200;
		//private readonly Polygon turtleShape;
		private ObservableCollection<DrawingInstruction> drawingInstructions = new ObservableCollection<DrawingInstruction>();

		private string parseError;
		private TimeSpan currentTime;

		public MainWindowViewModel(ParseProgramCommand parseProgramCommand)
		{
			parseProgramCommand.ParseResult += OnParseProgramCommandResult;
			DomainEvents.Register<ILogoCommandEvent>(Handle);
			//turtleShape = new Polygon();
			//turtleShape.ToTurtle(LogoController.CurrentSituation);
			//drawingInstructions.Add(turtleShape);
		}

		public MainWindowViewModel() :
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

		public ObservableCollection<DrawingInstruction> DrawingInstructions
		{
			get { return drawingInstructions; }
			set
			{
				drawingInstructions = value;
				OnPropertyChanged("DrawingInstructions");
			}
		}

		#region IDomainEventHandler<ILogoCommandEvent> Members

		public void Handle(ILogoCommandEvent e)
		{
			TurtleSituation currentSituation = LogoController.CurrentSituation;
			TurtleSituation newSituation = e.LogoCommand.CalculateSituation(currentSituation);

			switch (newSituation.Change)
			{
				case TurtleSituationChange.Moved:
					var line = new DrawingInstruction
					           	{
					           		X1 = currentSituation.Position.X,
					           		Y1 = currentSituation.Position.Y,
					           		X2 = newSituation.Position.X,
					           		Y2 = newSituation.Position.Y,
												TimeOffset = currentTime
					           	};
					drawingInstructions.Add(line);
					currentTime = currentTime.Add(TimeSpan.FromMilliseconds(TURTLE_SPEED_IN_MS));
					break;

				case TurtleSituationChange.Cleared:
					drawingInstructions.Clear();
					newSituation = TurtleSituation.DefaultSituation;
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

		#endregion

		private void OnParseProgramCommandResult(object sender, ParseErrorEventArgs e)
		{
			ParseError = e.ErrorMessage;
		}

		private void DoTurtle(TurtleSituation newSituation)
		{
			var rotateTransform = new RotateTransform(newSituation.TurnAngle)
			                      	{
			                      		CenterX = newSituation.Position.X,
			                      		CenterY = newSituation.Position.Y
			                      	};
			//turtleShape.ToTurtle(newSituation);
			//turtleShape.RenderTransform = rotateTransform;
			//drawingInstructions.Remove(turtleShape);
			//drawingInstructions.Add(turtleShape);
		}
	}
}