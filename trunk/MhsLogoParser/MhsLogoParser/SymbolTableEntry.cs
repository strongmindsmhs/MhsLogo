using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MhsLogoParser
{
	public class SymbolTableEntry
	{
		public static readonly SymbolTableEntry None = new NoneSymbolEntry();

		public class NoneSymbolEntry : SymbolTableEntry
		{
			public NoneSymbolEntry()
				: base(string.Empty)
			{
			}
		}

		private readonly List<SymbolTableAttribute> attributes = new List<SymbolTableAttribute>();
	
		public string Name { get; private set; }

		public SymbolTableEntry(string name)
		{
			Name = name;
		}

		public ReadOnlyCollection<SymbolTableAttribute> Attributes
		{
			get
			{
				return attributes.AsReadOnly();
			}
		}

		public void AddAttribute(SymbolTableAttribute symbolTableAttribute)
		{
			attributes.Add(symbolTableAttribute);
		}
	}
}