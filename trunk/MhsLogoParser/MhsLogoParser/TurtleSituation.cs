using System;

namespace MhsLogoParser
{
	public class TurtleSituation
	{
		private static readonly DefaultTurtleSituation defaultSituation = new DefaultTurtleSituation();
		public virtual int Angle { get; set; }
		public virtual int TurnAngle { get; set; }
		public virtual Position Position { get; set; }

		public static DefaultTurtleSituation DefaultSituation
		{
			get { return defaultSituation; }
		}

		#region Nested type: DefaultTurtleSituation

		public class DefaultTurtleSituation : TurtleSituation
		{
			public override int Angle
			{
				get { return 90; }
				set { throw new NotImplementedException(); }
			}

			public override Position Position
			{
				get { return new Position {X = 250, Y = 200}; }
				set { throw new NotImplementedException(); }
			}

			public override int TurnAngle
			{
				get { return 0; }
				set { throw new NotImplementedException(); }
			}
		}

		#endregion
	}
}