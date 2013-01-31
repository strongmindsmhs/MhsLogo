using System;
using System.Linq;

namespace MhsLogoParser
{
	public class LogoScanner
	{
		private readonly Token[] reserved = new[]
		                                    	{
		                                    		Token.CLEAR, Token.MOVETO, Token.FORWARD, Token.BACK, Token.RIGHT, Token.LEFT,
		                                    		Token.REPEAT, Token.TO, Token.END
		                                    	};

		#region Privates

		private readonly string rawContents;
		private char ch;
		private int idx;

		#endregion

		public LogoScanner(string input)
		{
			rawContents = input;
		}

		public string ScanBuffer { get; set; }

		public Token NextToken()
		{
			int oldIdx = idx;
			Token result = Scan();
			idx = oldIdx;
			return result;
		}

		public Token Scan()
		{
			while (idx < rawContents.Length)
			{
				ch = rawContents[idx];
				switch (ch)
				{
					case '[':
						idx++;
						return Token.LBRACKET;

					case ']':
						idx++;
						return Token.RBRACKET;

					case ',':
						idx++;
						return Token.COMMA;

					default:
						if (char.IsDigit(ch))
						{
							ScanBuffer = ch.ToString();
							idx++;
							while (idx < rawContents.Length)
							{
								ch = rawContents[idx];
								if (char.IsDigit(ch))
								{
									ScanBuffer += ch;
									idx++;
								}
								else break;
							}
							return Token.NUMBER;
						}
						else if (char.IsLetter(ch))
						{
							ScanBuffer = ch.ToString();
							idx++;
							while (idx < rawContents.Length)
							{
								ch = rawContents[idx];
								if (char.IsLetterOrDigit(ch) || ch == '_')
								{
									ScanBuffer += ch;
									idx++;
								}
								else break;
							}
							Token lookup;
							if (LookupReserved(ScanBuffer, out lookup))
							{
								return lookup;
							}
							return Token.IDENTIFIER;
						}
						else if (char.IsWhiteSpace(ch))
						{
							idx++;
						}
						else
						{
							LexicalError(String.Format("Lexical error at '{0}' ('{1}')", ch, ScanBuffer));
						}
						break;
				}
			}
			return Token.EOF;
		}

		private static void LexicalError(string errorMessage)
		{
			throw new LogoScannerException(errorMessage);
		}

		private bool LookupReserved(string s, out Token lookup)
		{
			lookup = TokenHelper.TextToToken(s);

			return reserved.Contains(lookup);
		}
	}
}