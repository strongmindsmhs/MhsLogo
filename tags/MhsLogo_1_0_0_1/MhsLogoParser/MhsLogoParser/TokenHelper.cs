using System;
using System.Reflection;

namespace MhsLogoParser
{
	public static class TokenHelper
	{
		public static Token TextToToken(string s)
		{
			Type tokenType = typeof (Token);
			MemberInfo[] fields = tokenType.GetMembers(BindingFlags.Public | BindingFlags.Static);
			foreach (MemberInfo memberInfo in fields)
			{
				object[] attr = memberInfo.GetCustomAttributes(typeof (TokenAsTextAttribute), false);
				if (attr.Length > 0)
				{
					string text = ((TokenAsTextAttribute) attr[0]).TokenAsText;
					if (text.Equals(s))
					{
						return (Token) Enum.Parse(tokenType, memberInfo.Name);
					}
				}
			}
			return Token.NONE;
		}

		public static string TokenToText(Token token)
		{
			Type tokenType = typeof (Token);
			MemberInfo[] field = tokenType.GetMember(Enum.GetName(tokenType, token));
			object[] attr = field[0].GetCustomAttributes(typeof (TokenAsTextAttribute), false);
			if (attr.Length > 0)
			{
				return ((TokenAsTextAttribute) attr[0]).TokenAsText;
			}
			return token.ToString();
		}
	}
}