using GSharpProject;
using GSharpProject.Parsing;

namespace GSharpTest.Parser;

public class ParserTest
{
    [Theory]
    [InlineData("3;","3")]
    [InlineData("\"aas\";","aas")]
    public void ParseLiteralNumberEvalOk(string input, string result)
    {
        var statements = StatementsTree.Create(input);

        Assert.Single(statements);
        Assert.IsType<GSharpLiteralExpression>(statements[0]);
        Assert.Equal(result, ((GSharpLiteralExpression)statements[0]).LiteralToken.Text);
    }
}