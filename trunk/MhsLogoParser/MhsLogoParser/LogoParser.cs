using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MhsUtility;

namespace MhsLogoParser
{
	public class LogoParser
	{
		private readonly LogoScanner scanner;
		private readonly SymbolTable symbolTable;

		public LogoParser(LogoScanner scanner, SymbolTable symbolTable)
		{
			this.scanner = scanner;
			this.symbolTable = symbolTable;
		}

		// <logo-program>  ::= TO <identifier> <logo-sentence> { <logo-sentence> } END <EOF>
		//								   | <logo-sentence> { <logo-sentence> } <EOF>
		public ICollection<BaseLogoCommand> ParseLogoProgram()
		{
			var result = new Collection<BaseLogoCommand>();
			Token nextToken = scanner.NextToken();
			if (nextToken == Token.TO)
			{
				Match(Token.TO);
				Match(Token.IDENTIFIER);
				var entry = symbolTable.Enter(scanner.ScanBuffer);
				var routineCommands = new List<BaseLogoCommand>();
				routineCommands.Add(ParseLogoSentence());
				for (Token token = scanner.NextToken(); IsSentencePrefix(token); token = scanner.NextToken())
				{
					routineCommands.Add(ParseLogoSentence());
				}
				var routineAttribute = new SymbolTableRoutineAttribute(routineCommands);
				entry.AddAttribute(routineAttribute);
				Match(Token.END);
				DomainEvents.Raise(new LogoRoutineEvent(symbolTable.LookupRoutines()));
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

		// <logo-sentence>
		private BaseLogoCommand ParseLogoSentence()
		{
			BaseLogoCommand result = null;
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
		private BaseLogoCommand ParseLogoClearCommand()
		{
			Match(Token.CLEAR);
			return new LogoClearCommand();
		}

		// <logo-sentence> ::= MOVETO <integer> , <integer>
		private BaseLogoCommand ParseLogoMoveToCommand()
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
		private BaseLogoCommand ParseLogoDirectionCommand(Token nextToken)
		{
			BaseLogoCommand result;
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
		private BaseLogoCommand ParseLogoRepeatCommand()
		{
			Match(Token.REPEAT);
			Match(Token.NUMBER);
			var repeatNumberRecord = new NumberRecord(Token.REPEAT, scanner.ScanBuffer);
			Match(Token.LBRACKET);
			var logoCommands = new List<BaseLogoCommand>();
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