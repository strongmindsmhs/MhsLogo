using System;
using System.Runtime.Serialization;

namespace MhsLogoParser
{
	public class LogoSyntaxErrorException : ApplicationException
	{
		public LogoSyntaxErrorException(LogoErrorCode errorCode, string scanBuffer)
		{
			ErrorCode = errorCode;
			ScanBuffer = scanBuffer;
		}

		public LogoSyntaxErrorException(string message, LogoErrorCode errorCode, string scanBuffer)
			: base(message)
		{
			ErrorCode = errorCode;
			ScanBuffer = scanBuffer;
		}

		public LogoSyntaxErrorException(string message, Exception innerException, LogoErrorCode errorCode, string scanBuffer)
			: base(message, innerException)
		{
			ErrorCode = errorCode;
			ScanBuffer = scanBuffer;
		}

		protected LogoSyntaxErrorException(SerializationInfo info, StreamingContext context, LogoErrorCode errorCode,
		                                   string scanBuffer) : base(info, context)
		{
			ErrorCode = errorCode;
			ScanBuffer = scanBuffer;
		}

		public LogoErrorCode ErrorCode { get; private set; }

		public string ScanBuffer { get; private set; }
	}
}