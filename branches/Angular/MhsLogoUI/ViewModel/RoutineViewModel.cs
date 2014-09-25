using System.Collections.ObjectModel;
using MhsLogoParser;
using MhsUtility;

namespace MhsLogoUI.ViewModel
{
	public class RoutineViewModel : BaseViewModel, IDomainEventHandler<ILogoRoutineEvent>
	{
		private readonly ObservableCollection<string> routines = new ObservableCollection<string>();

		public RoutineViewModel()
		{
			DomainEvents.Register<ILogoRoutineEvent>(Handle);
		}

		public ObservableCollection<string> Routines
		{
			get { return routines; }
		}

		#region IDomainEventHandler<ILogoRoutineEvent> Members

		public void Handle(ILogoRoutineEvent args)
		{
			routines.Clear();
			foreach (string name in args.RoutineNames)
			{
				routines.Add(name);
			}
		}

		#endregion
	}
}