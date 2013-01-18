using System;
using System.Runtime.Serialization;

namespace MhsLogoParser
{
	public class LogoScannerException : ApplicationException
	{
		public LogoScannerException()
		{
		}

		public LogoScannerException(string message) : base(message)
		{
		}

		public LogoScannerException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected LogoScannerException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}