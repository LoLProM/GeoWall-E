namespace GSharpProject.Parsing
{
	public static class StatementsTree
	{
		public static GSharpStatementsCollection Create(string text)
		{
			var parser = new Parser(text);
			return parser.Parse();  
		}
	}
}
