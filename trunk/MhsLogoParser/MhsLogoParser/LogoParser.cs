using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MhsLogoParser.LogoCommands;
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

		// <logo-program>  ::= <logo-routine-declaration> <logo-sentences> | <logo-sentences> 
		public ICollection<BaseLogoCommand> ParseLogoProgram()
		{
			var result = new Collection<BaseLogoCommand>();
			Token nextToken = scanner.NextToken();
			if (nextToken == Token.TO)
			{
				ParseLogoRoutineDeclaration();
				if (IsSentencePrefix(scanner.NextToken()))
				{
					ParseLogoSentences(result);
				}
			}
			else
			{
				ParseLogoSentences(result);
			}
			Match(Token.EOF);
			return result;
		}

		// <logo-routine-declaration> ::= TO <identifier> <logo-sentences> END <EOF>
		private void ParseLogoRoutineDeclaration()
		{
			Match(Token.TO);
			Match(Token.IDENTIFIER);
			SymbolTableEntry entry = symbolTable.Enter(scanner.ScanBuffer);
			var routineCommands = new Collection<BaseLogoCommand>();
			ParseLogoSentences(routineCommands);
			var routineAttribute = new SymbolTableRoutineAttribute(routineCommands);
			entry.AddAttribute(routineAttribute);
			Match(Token.END);
			DomainEvents.Raise(new LogoRoutineEvent(symbolTable.LookupRoutines()));
		}

		// <logo-sentences>  ::= <logo-sentence> { <logo-sentence> } <EOF>
		private void ParseLogoSentences(Collection<BaseLogoCommand> result)
		{
			result.Add(ParseLogoSentence());
			for (Token token = scanner.NextToken(); IsSentencePrefix(token); token = scanner.NextToken())
			{
				result.Add(ParseLogoSentence());
			}
		}

		// <logo-sentence>  ::= <logo-clear-command> 
		//									  | <logo-move-command>
		//									  | <logo-direction-command>
		//									  | <logo-repeat-command>
		//									  | <logo-routine-call-command>
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

				case Token.IDENTIFIER:
					result = ParseLogoRoutineCall();
					break;

				default:
					SyntaxError(
						String.Format("Expected one of: CLEAR, MOVETO, FORWARD, BACK, LEFT, RIGHT, REPEAT or IDENTIFIER but found {0}",
						              TokenHelper.TokenToText(nextToken)), LogoErrorCode.SentenceError);
					break;
			}
			return result;
		}

		// <logo-routine-call-command> ::= <identifier>
		private BaseLogoCommand ParseLogoRoutineCall()
		{
			Match(Token.IDENTIFIER);
			SymbolTableEntry routineEntry = symbolTable.Lookup(scanner.ScanBuffer);
			SymbolTableRoutineAttribute routineAttribute = null;
			if (!routineEntry.TryLookupRoutineAttribute(ref routineAttribute))
			{
				SyntaxError(String.Format("Unknown routine name {0}", scanner.ScanBuffer), LogoErrorCode.RoutineCallError);
			}
			return new LogoRoutineCallCommand(scanner.ScanBuffer, routineAttribute.Commands);
		}

		// <logo-clear-command> ::= CLEAR
		private BaseLogoCommand ParseLogoClearCommand()
		{
			Match(Token.CLEAR);
			return new LogoClearCommand();
		}

		// <logo-move-command> ::= MOVETO <integer> , <integer>
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

		// <logo-direction-command> ::= FORWARD <integer>
		//														| BACK <integer>
		//														| LEFT <integer>
		//														| RIGHT <integer>
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

		// <logo-repeat-command> ::= REPEAT <integer> [ <logo-sentence> { <logo-sentence> } ]
		private BaseLogoCommand ParseLogoRepeatCommand()
		{
			Match(Token.REPEAT);
			Match(Token.NUMBER);
			var repeatNumberRecord = new NumberRecord(Token.REPEAT, scanner.ScanBuffer);
			Match(Token.LBRACKET);
			var logoCommands = new Collection<BaseLogoCommand>();
			ParseLogoSentences(logoCommands);
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
			       token == Token.REPEAT || token == Token.IDENTIFIER;
		}
	}
}