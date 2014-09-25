using System;

namespace MhsLogoUI.ViewModel
{
	public class DrawingInstruction : BaseInstruction
	{
		private TimeSpan timeOffset;

		private int x1;
		private int x2;

		private int y1;
		private int y2;

		public TimeSpan TimeOffset
		{
			get { return timeOffset; }
			set
			{
				timeOffset = value;
				OnPropertyChanged("TimeOffset");
			}
		}

		public int X1
		{
			get { return x1; }
			set
			{
				x1 = value;
				OnPropertyChanged("X1");
			}
		}

		public int Y1
		{
			get { return y1; }
			set
			{
				y1 = value;
				OnPropertyChanged("Y1");
			}
		}

		public int X2
		{
			get { return x2; }
			set
			{
				x2 = value;
				OnPropertyChanged("X2");
			}
		}

		public int Y2
		{
			get { return y2; }
			set
			{
				y2 = value;
				OnPropertyChanged("Y2");
			}
		}
	}
}