using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class LogoParser
	{
		private readonly LogoScanner scanner;

		public LogoParser(LogoScanner logoScanner)
		{
			scanner = logoScanner;
		}

		// <logo-program>  ::= <logo-sentence> { <logo-sentence> } <EOF>
		public ICollection<ILogoCommand> ParseLogoProgram()
		{
			var result = new Collection<ILogoCommand>();
			result.Add(ParseLogoSentence());
			while (true)
			{
				switch (scanner.NextToken())
				{
					case Token.FORWARD:
					case Token.BACK:
					case Token.LEFT:
					case Token.RIGHT:
					case Token.REPEAT:
						result.Add(ParseLogoSentence());
						break;

					default:
						Match(Token.EOF);
						return result;
				}
			}
		}

		// <logo-sentence> ::= FORWARD <integer>
		//                   | BACK <integer>
		//                   | LEFT <integer>
		//                   | RIGHT <integer>
		//                   | REPEAT <integer> [ <logo-sentence> { <logo-sentence> } ]
		private ILogoCommand ParseLogoSentence()
		{
			ILogoCommand result = null;
			Token nextToken = scanner.NextToken();
			switch (nextToken)
			{
				case Token.FORWARD:
				case Token.BACK:
				case Token.LEFT:
				case Token.RIGHT:
					Match(nextToken);
					Match(Token.NUMBER);
					var numberRecord = new NumberRecord(nextToken, scanner.ScanBuffer);
					if (nextToken == Token.FORWARD || nextToken == Token.BACK)
					{
						result = new LogoMoveCommand(numberRecord);
					}
					else
					{
						result = new LogoTurnCommand(numberRecord);
					}
					break;

				case Token.REPEAT:
					Match(nextToken);
					Match(Token.NUMBER);
					var repeatNumberRecord = new NumberRecord(Token.REPEAT, scanner.ScanBuffer);
					Match(Token.LBRACKET);
					var logoCommands = new List<ILogoCommand>();
					logoCommands.Add(ParseLogoSentence());
					for (Token token = scanner.NextToken();
					     token == Token.FORWARD || token == Token.BACK || token == Token.LEFT || token == Token.RIGHT ||
					     token == Token.REPEAT;
					     token = scanner.NextToken())
					{
						logoCommands.Add(ParseLogoSentence());
					}
					Match(Token.RBRACKET);
					result = new LogoRepeatCommand(repeatNumberRecord, logoCommands);
					break;

				default:
					SyntaxError(String.Format("Expected one of: FORWARD, BACK, LEFT, RIGHT or REPEAT but found {0}",
					                          TokenHelper.TokenToText(nextToken)), LogoErrorCode.SentenceError);
					break;
			}
			return result;
		}

		private void Match(Token token)
		{
			Token nextToken = scanner.Scan();
			if (nextToken != token)
			{
				SyntaxError(String.Format("Expected {0} but found {1} (at '{2}')",
				                          TokenHelper.TokenToText(token),
				                          TokenHelper.TokenToText(nextToken),
				                          scanner.ScanBuffer), LogoErrorCode.MatchError);
			}
		}

		private void SyntaxError(string errorMessage, LogoErrorCode errorCode)
		{
			throw new LogoSyntaxErrorException(errorMessage, errorCode, scanner.ScanBuffer);
		}
	}
}