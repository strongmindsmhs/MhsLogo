using System;
using System.ComponentModel;

namespace MhsLogoUI.ViewModel
{
	public class DrawingInstruction: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

		private TimeSpan timeOffset;
		public TimeSpan TimeOffset
		{
			get { return timeOffset; }
			set
			{
				timeOffset = value;
				OnPropertyChanged("TimeOffset");
			}
		}

		private int x1;
		public int X1
		{
			get { return x1; }
			set { 
				x1 = value;
				OnPropertyChanged("X1");
			}
		}

		private int y1;
		public int Y1
		{
			get { return y1; }
			set
			{
				y1 = value;
				OnPropertyChanged("Y1");
			}
		}

		private int x2;
		public int X2
		{
			get { return x2; }
			set
			{
				x2 = value;
				OnPropertyChanged("X2");
			}
		}

		private int y2;
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