using MhsLogoParser;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestLogoScanner
	{
		[Test]
		public void CanHandleWhiteSpace()
		{
			const string INPUT = "FORWARD 100\r\n\t";
			var expectedTokens = new[] {Token.FORWARD, Token.NUMBER, Token.EOF};
			var scanner = new LogoScanner(INPUT);
			foreach (Token expectedToken in expectedTokens)
			{
				Token token = scanner.Scan();
				Assert.AreEqual(expectedToken, token);
			}
		}

		[Test]
		public void CanRecognizeTokens()
		{
			const string INPUT = "CLEAR TO Rectangle REPEAT 4 [ FORWARD 100 LEFT 90 ] END MOVETO 200,50";
			var expectedTokens = new[]
			                     	{
			                     		Token.CLEAR, Token.TO, Token.IDENTIFIER, Token.REPEAT, Token.NUMBER, Token.LBRACKET,
			                     		Token.FORWARD, Token.NUMBER, Token.LEFT, Token.NUMBER, Token.RBRACKET, Token.END,
			                     		Token.MOVETO, Token.NUMBER, Token.COMMA, Token.NUMBER, Token.EOF
			                     	};
			var scanner = new LogoScanner(INPUT);
			foreach (Token expectedToken in expectedTokens)
			{
				Token token = scanner.Scan();
				Assert.AreEqual(expectedToken, token);
			}
		}
	}
}