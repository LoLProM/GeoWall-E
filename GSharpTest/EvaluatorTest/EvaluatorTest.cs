using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;

namespace GSharpTest.Evaluator;

public class EvaluatorTest
{
    [Fact]
    public void EvaluateLiteralNumberEvalOk()
    {
        var statements = StatementsTree.Create("3;");

        var evaluator = new GSharpEvaluator(statements.Statements[0]);

        var result = evaluator.Evaluate();

        Assert.Single(statements.Statements);
        Assert.IsType<GSharpLiteralExpression>(statements.Statements[0]);
        Assert.Equal((double)3, result);

    }

    [Theory]
    [InlineData("let a = 2; in a;",2)]
    [InlineData("a = 2; f(p) = 2+p; f(a);",4)]//probar despues
    [InlineData("let a = sin(0); in a;",0)]
    [InlineData("point p; point m; let a = measure(p,m); in a;",0)]
    [InlineData("point p = (0,0); point m = (0,1); let a = measure(p,m); in a;",1)]
    [InlineData("let a = if let b = 2; in b == --(2*1) then 3 else 1; in 2+(a * 2 )/6;",3)]
    [InlineData("(let c = 3; in c) + 4;",7)]

    public void EvalLetInIsOk(string input, double expectedResult)
    {
        var statements = StatementsTree.Create(input);
        GSharpEvaluator evaluator;
        object result = 0;
		foreach (var i in statements.Statements)
		{
			if (i is FunctionDeclarationExpression)
			{
				continue;
			}
			evaluator = new GSharpEvaluator(i);
			result = evaluator.Evaluate();
		}

        Assert.Equal(expectedResult,result);
    }
}