using System;
using System.ComponentModel;

namespace MhsLogoUI.ViewModel
{
	public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
	{
		public virtual string DisplayName { get; protected set; }

		#region IDisposable Members

		public void Dispose()
		{
			OnDispose();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				handler(this, e);
			}
		}

		protected virtual void OnDispose()
		{
		}
	}
}