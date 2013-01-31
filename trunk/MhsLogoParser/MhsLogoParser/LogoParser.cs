﻿using System;
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

		// <logo-program>  ::= TO <identifier> <logo-sentence> { <logo-sentence> } END <EOF>
		//								   | <logo-sentence> { <logo-sentence> } <EOF>
		public ICollection<ILogoCommand> ParseLogoProgram()
		{
			var result = new Collection<ILogoCommand>();
			Token nextToken = scanner.NextToken();
			if (nextToken == Token.TO)
			{
				Match(Token.TO);
				Match(Token.IDENTIFIER);
				var identifierRecord = new IdentifierRecord(scanner.ScanBuffer);
				var routineCommands = new List<ILogoCommand>();
				routineCommands.Add(ParseLogoSentence());
				for (Token token = scanner.NextToken(); IsSentencePrefix(token); token = scanner.NextToken())
				{
					routineCommands.Add(ParseLogoSentence());
				}
				Match(Token.END);
				var routineCommand = new LogoDefineRoutineCommand(identifierRecord, routineCommands);
				result.Add(routineCommand);
			}
			else
			{
				result.Add(ParseLogoSentence());
				for (Token token = scanner.NextToken(); IsSentencePrefix(token); token = scanner.NextToken())
				{
					result.Add(ParseLogoSentence());
				}
			}
			Match(Token.EOF);
			return result;
		}

		private ILogoCommand ParseLogoSentence()
		{
			ILogoCommand result = null;
			Token nextToken = scanner.NextToken();
			switch (nextToken)
			{
				case Token.CLEAR:
					result = ParseLogoClearCommand();
					break;

				case Token.MOVETO:
					result = ParseLogoMoveToCommand();
					break;

				case Token.FORWARD:
				case Token.BACK:
				case Token.LEFT:
				case Token.RIGHT:
					result = ParseLogoDirectionCommand(nextToken);
					break;

				case Token.REPEAT:
					result = ParseLogoRepeatCommand();
					break;

				default:
					SyntaxError(String.Format("Expected one of: CLEAR, MOVETO, FORWARD, BACK, LEFT, RIGHT or REPEAT but found {0}",
					                          TokenHelper.TokenToText(nextToken)), LogoErrorCode.SentenceError);
					break;
			}
			return result;
		}

		// <logo-sentence> ::= CLEAR
		private ILogoCommand ParseLogoClearCommand()
		{
			Match(Token.CLEAR);
			return new LogoClearCommand();
		}

		// <logo-sentence> ::= MOVETO <integer> , <integer>
		private ILogoCommand ParseLogoMoveToCommand()
		{
			Match(Token.MOVETO);
			Match(Token.NUMBER);
			var numberXRecord = new NumberRecord(Token.MOVETO, scanner.ScanBuffer);
			Match(Token.COMMA);
			Match(Token.NUMBER);
			var numberYRecord = new NumberRecord(Token.MOVETO, scanner.ScanBuffer);
			return new LogoPositionCommand(numberXRecord, numberYRecord);
		}

		// <logo-sentence> ::= FORWARD <integer>
		//                   | BACK <integer>
		//                   | LEFT <integer>
		//                   | RIGHT <integer>
		private ILogoCommand ParseLogoDirectionCommand(Token nextToken)
		{
			ILogoCommand result;
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
			return result;
		}

		// <logo-sentence> ::= REPEAT <integer> [ <logo-sentence> { <logo-sentence> } ]
		private ILogoCommand ParseLogoRepeatCommand()
		{
			Match(Token.REPEAT);
			Match(Token.NUMBER);
			var repeatNumberRecord = new NumberRecord(Token.REPEAT, scanner.ScanBuffer);
			Match(Token.LBRACKET);
			var logoCommands = new List<ILogoCommand>();
			logoCommands.Add(ParseLogoSentence());
			for (Token token = scanner.NextToken(); IsSentencePrefix(token); token = scanner.NextToken())
			{
				logoCommands.Add(ParseLogoSentence());
			}
			Match(Token.RBRACKET);
			return new LogoRepeatCommand(repeatNumberRecord, logoCommands);
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

		private static bool IsSentencePrefix(Token token)
		{
			return token == Token.CLEAR || token == Token.MOVETO ||
			       token == Token.FORWARD || token == Token.BACK ||
			       token == Token.LEFT || token == Token.RIGHT ||
			       token == Token.REPEAT;
		}
	}
}