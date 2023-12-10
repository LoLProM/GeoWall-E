using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;

namespace GSharpTest.Parser;

public class ParserTest
{
    // [Theory]
    // [InlineData("3;","3")]
    // [InlineData("\"aas\";","aas")] en el metodo poner input y result
    [Fact]
    public void ParseLiteralNumberEvalOk()
    {
        var statements = StatementsTree.Create("3;");

        Assert.Single(statements);
        Assert.IsType<GSharpLiteralExpression>(statements[0]);
        Assert.Equal("3",((GSharpLiteralExpression)statements[0]).LiteralToken.Text);
    
    }

    [Fact]
    public void ParseLetInIsOk()
    {
        var statements = StatementsTree.Create("let a = 2; b = 2; in a + b;");

        Assert.Single(statements);
        Assert.IsType<Let_In_Expression>(statements[0]);

        // GSharpEvaluator evaluator;
		// object result;
		// foreach (var i in statements)
		// {
		// 	if (i is FunctionDeclarationExpression)
		// 	{
		// 		continue;
		// 	}
		// 	evaluator = new GSharpEvaluator(i);
		// 	result = evaluator.Evaluate();
		// }
    }
    
    [Fact]
    public void ParseLiteralSequenceEvalOk()
    {
        // var statements = StatementsTree.Create("{1...};");
        var statements = StatementsTree.Create("{1,3,4,53};");


        Assert.Single(statements);
        Assert.IsType<GSharpLiteralSequence>(statements[0]);
    }
}