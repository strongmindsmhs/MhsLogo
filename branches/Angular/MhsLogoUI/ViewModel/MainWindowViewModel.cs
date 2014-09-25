using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MhsLogoController;
using MhsLogoParser;
using MhsLogoUI.Commands;
using MhsUtility;

namespace MhsLogoUI.ViewModel
{
	public class MainWindowViewModel : BaseViewModel, IDomainEventHandler<ILogoCommandEvent>
	{
		private const int TURTLE_SPEED_IN_MS = 20;

		private readonly TurtleInstruction turtleInstruction;
		private TimeSpan currentTime;
		private BindingList<BaseInstruction> drawingInstructions = new BindingList<BaseInstruction>();
		private string parseError;

		public MainWindowViewModel(ParseProgramCommand parseProgramCommand)
		{
			parseProgramCommand.ParseResult += OnParseProgramCommandResult;
			DomainEvents.Register<ILogoCommandEvent>(Handle);
			turtleInstruction = new TurtleInstruction(LogoController.CurrentSituation);
			drawingInstructions.Add(turtleInstruction);
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

		public BindingList<BaseInstruction> DrawingInstructions
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
					DrawingInstructions.Add(line);
					break;

				case TurtleSituationChange.Cleared:
					newSituation = TurtleSituation.DefaultSituation;
					List<BaseInstruction> itemsToRemove = DrawingInstructions.Where(i => i is DrawingInstruction).ToList();
					foreach (BaseInstruction itemToRemove in itemsToRemove)
					{
						DrawingInstructions.Remove(itemToRemove);
					}
					break;

				case TurtleSituationChange.Positioned:
				case TurtleSituationChange.Turned:
				case TurtleSituationChange.None:
					// Do nothing
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}

			DoTurtle(currentSituation, newSituation);

			currentTime = currentTime.Add(TimeSpan.FromMilliseconds(TURTLE_SPEED_IN_MS));

			LogoController.CurrentSituation = newSituation;
		}

		#endregion

		private void OnParseProgramCommandResult(object sender, ParseErrorEventArgs e)
		{
			ParseError = e.ErrorMessage;
			if (!e.Error)
			{
				currentTime = TimeSpan.FromMilliseconds(0);
			}
		}

		private void DoTurtle(TurtleSituation currentSituation, TurtleSituation newSituation)
		{
			turtleInstruction.CenterX = newSituation.Position.X;
			turtleInstruction.CenterY = newSituation.Position.Y;
			turtleInstruction.TurnAngle = newSituation.TurnAngle;
			turtleInstruction.TimeOffset = currentTime;
			turtleInstruction.ToPoints(newSituation);
			turtleInstruction.Movement =
				new PathGeometry(
					new PathFigureCollection
						{
							new PathFigure(new Point(currentSituation.Position.X, currentSituation.Position.Y),
							               new List<PathSegment>
							               	{
							               		new LineSegment(
							               			new Point(newSituation.Position.X, newSituation.Position.Y), false)
							               	}, false)
						});
		}
	}
}