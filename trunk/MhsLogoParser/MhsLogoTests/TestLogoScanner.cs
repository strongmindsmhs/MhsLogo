using MhsLogoParser;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestLogoScanner
	{
		[Test]
		public void CanRecognizeTokens()
		{
			const string input = "REPEAT 4 [ FORWARD 100 LEFT 90 ]";
			var expectedTokens = new[]
			                     	{
			                     		Token.REPEAT, Token.NUMBER, Token.LBRACKET, Token.FORWARD, Token.NUMBER, Token.LEFT,
			                     		Token.NUMBER, Token.RBRACKET, Token.EOF
			                     	};
			var scanner = new LogoScanner(input);
			foreach (Token expectedToken in expectedTokens)
			{
				Token token = scanner.Scan();
				Assert.AreEqual(expectedToken, token);
			}
		}
	}
}