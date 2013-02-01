using MhsLogoParser;
using NUnit.Framework;

namespace MhsLogoTests
{
	[TestFixture]
	public class TestSymbolTable
	{
		#region Setup/Teardown

		[SetUp]
		protected void SetUp()
		{
			sut = new SymbolTable();
		}

		#endregion

		private SymbolTable sut;

		[Test]
		public void CanCreateSymbolTable()
		{
			Assert.AreEqual(0, sut.NumberOfEntries);
		}

		[Test]
		public void CanEnterEntry()
		{
			SymbolTableEntry entry = sut.Enter("ABC");
			Assert.IsNotNull(entry);
			Assert.AreEqual("ABC", entry.Name);
			Assert.AreEqual(1, sut.NumberOfEntries);
		}

		[Test]
		public void CanHandleExistingEntry()
		{
			SymbolTableEntry entry1 = sut.Enter("ABC");
			SymbolTableEntry entry2 = sut.Enter("ABC");
			Assert.AreEqual("ABC", entry1.Name);
			Assert.AreEqual("ABC", entry2.Name);
			Assert.AreEqual(1, sut.NumberOfEntries);
			Assert.AreSame(entry1, sut.Lookup("ABC"));
		}

		[Test]
		public void CanLookupEntry()
		{
			sut.Enter("ABC");
			SymbolTableEntry entry = sut.Lookup("ABC");
			Assert.AreEqual("ABC", entry.Name);
			Assert.AreEqual(SymbolTableEntry.None, sut.Lookup("ABE"));
		}
	}
}