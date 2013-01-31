using MhsLogoParser;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestLogoParser
	{
		private static void TestParseProgram(string program)
		{
			var parser = new LogoParser(new LogoScanner(program));
			parser.ParseLogoProgram();
		}

		private static LogoSyntaxErrorException TestParseProgramThrows(string program)
		{
			return Assert.Throws<LogoSyntaxErrorException>(
				() => TestParseProgram(program)
				);
		}

		[Test]
		public void CanParseRoutine()
		{
			TestParseProgram("TO RECTANGLE REPEAT 4 [ FORWARD 100 LEFT 90 ] END");
		}

		[Test]
		public void CanParseSimpleLogoProgram()
		{
			TestParseProgram("FORWARD 50");
		}

		[Test]
		public void CanThrowOnMissingRBracketSyntaxError()
		{
			LogoSyntaxErrorException exception = TestParseProgramThrows("REPEAT 4 [ FORWARD 50");
			Assert.AreEqual(LogoErrorCode.MatchError, exception.ErrorCode);
			Assert.AreEqual("50", exception.ScanBuffer);
		}

		[Test]
		public void CanThrowOnSecondSentenceWithSyntaxError()
		{
			LogoSyntaxErrorException exception = TestParseProgramThrows("FORWARD 50 LEFT");
			Assert.AreEqual(LogoErrorCode.MatchError, exception.ErrorCode);
			Assert.AreEqual("LEFT", exception.ScanBuffer);
		}

		[Test]
		public void CanThrowOnSimpleSyntaxError()
		{
			LogoSyntaxErrorException exception = TestParseProgramThrows("50 FORWARD");
			Assert.AreEqual(LogoErrorCode.SentenceError, exception.ErrorCode);
			Assert.AreEqual("50", exception.ScanBuffer);
		}
	}
}