using System;

namespace MhsLogoParser
{
	public enum Token
	{
		[TokenAsText("REPEAT")] REPEAT,
		[TokenAsText("FORWARD")] FORWARD,
		[TokenAsText("BACK")] BACK,
		[TokenAsText("LEFT")] LEFT,
		[TokenAsText("RIGHT")] RIGHT,
		[TokenAsText("NUMBER")] NUMBER,
		[TokenAsText("CLEAR")] CLEAR,
		[TokenAsText("MOVETO")] MOVETO,
		[TokenAsText(",")] COMMA,
		[TokenAsText("[")] LBRACKET,
		[TokenAsText("]")] RBRACKET,
		EOF,
		NONE,
	}

	public class TokenAsTextAttribute : Attribute
	{
		private readonly string tokenAsText;

		public TokenAsTextAttribute(string tokenAsText)
		{
			this.tokenAsText = tokenAsText;
		}

		public string TokenAsText
		{
			get { return tokenAsText; }
		}
	}
}