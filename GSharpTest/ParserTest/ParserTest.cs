using GSharpProject;
using GSharpProject.Parsing;
namespace GSharpTest.Parser;

public class ParserTest
{
    [Fact]
    public void ParseLiteralNumberEvalOk()
    {
        var statements = StatementsTree.Create("3;");
        TypeChecker.CheckType(statements);
        var a = statements.Statements;
        Assert.Single(a);
        Assert.IsType<GSharpLiteralExpression>(a[0]);
        Assert.Equal("3",((GSharpLiteralExpression)a[0]).LiteralToken.Text);
    }

    [Theory]
    [InlineData("-1;",typeof(GSharpUnaryExpression))]
    [InlineData("---1;",typeof(GSharpUnaryExpression))]
    [InlineData("--(2*1);",typeof(GSharpUnaryExpression))]
    [InlineData("----1;",typeof(GSharpUnaryExpression))]

    public void ParseUnaryExpressionEvalOk(string input, Type result)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        Assert.Equal(result,statements.Statements[0].GetType());
    }

    [Theory]
    [InlineData(" let a = let b = 4; in b + a; in a + 5;",typeof(Let_In_Expression))]
    [InlineData("let a = 2; in let a = 4; in a;",typeof(Let_In_Expression))]
    [InlineData("let a = let b = 2; in b; in a *2;",typeof(Let_In_Expression))]
    [InlineData("F(x) = if x == 1 then 1 else x; let a = if let b = 2; in b == --(2*1) then 3 else 1; in F(3-2) * 2 /12;",typeof(Let_In_Expression))]
    [InlineData("F(x) = if x == 1 then 1 else x; F({1,2,3}) + 2;",typeof(GSharpBinaryExpression))]
    public void ParseLetInIsOk(string input, Type result)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.Equal(result,statementss[0].GetType());
    }

    [Theory]
    [InlineData("point p;",typeof(GSharpPointExpression))]
    [InlineData("ray p;",typeof(GSharpRayExpression))]
    [InlineData("arc p;",typeof(GSharpArcExpression))]
    [InlineData("circle p;",typeof(GSharpCircleExpression))]
    [InlineData("line p;",typeof(GSharpLineExpression))]
    [InlineData("segment p;",typeof(GSharpSegmentExpression))]
    [InlineData("point p = (2,3);",typeof(GSharpPointExpression))]
    [InlineData("point p = (let a = 2; in a, 2+3);",typeof(GSharpPointExpression))]
    [InlineData("point m; point a; ray p = (m,a);",typeof(GSharpRayExpression))]
    [InlineData("circle p = (let a = 2; in a, 2+3);",typeof(GSharpCircleExpression))]
    [InlineData("line p = (let a = 2; in a, 2+3);",typeof(GSharpLineExpression))]
    [InlineData("segment p = (let a = 2; in a, 2+3);",typeof(GSharpSegmentExpression))]
    [InlineData("arc p = (let a = 2; in a, 2+3);",typeof(GSharpArcExpression))]


    public void ParseFiguresEvalOk(string input, Type result)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.Single(statementss);
        Assert.Equal(result,statementss[0].GetType());
    }

    [Fact]
    public void ParseIfElseEvalOk()
    {
        var statements = StatementsTree.Create("if 2 > 3 then 1 else 3;");
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.Single(statementss);
        Assert.IsType<If_ElseStatement>(statementss[0]);
    }

    [Fact]
    public void ParseRecursiveFunctionsEvalOk()
    {
        var statements = StatementsTree.Create("fact(x) = if x == 1 then 1 else x * fact(x-1);");
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.Single(statementss);
        Assert.IsType<FunctionDeclarationExpression>(statementss[0]);
    }

    [Fact]
    public void ParseFunctionsEvalOk()//Revisar
    {
        var statements = StatementsTree.Create("F(x) = if x == 1 then 1 else x; a = 1;");
    
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.IsType<FunctionDeclarationExpression>(statementss[0]);
    }
    
    [Fact]
    public void ParseLiteralSequenceEvalOk()
    {
        var statements = StatementsTree.Create("{1,3,4,53};");
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();

        Assert.Single(statementss);
        Assert.IsType<GSharpLiteralSequence>(statementss[0]);
    }
    
    [Fact]
    public void ParseRangeSequenceEvalOk()
    {
        var statements = StatementsTree.Create("{1...5};");
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();

        Assert.Single(statementss);
        Assert.IsType<GSharpRangeSequence>(statementss[0]);
    }

    [Fact]
    public void ParseInfiniteSequenceEvalOk()
    {
        var statements = StatementsTree.Create("{1...};");
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();

        Assert.Single(statementss);
        Assert.IsType<GSharpInfiniteSequence>(statementss[0]);
    }

    [Theory]
    [InlineData("a,b,_ = {1,2,3};",typeof(AssignmentExpression), typeof(GSharpLiteralSequence))]
    [InlineData("a,b,_ = undefine;",typeof(AssignmentExpression), typeof(GSharpLiteralExpression))]

    public void CheckParseAssignment(string input, Type result, Type assignmentInnerExpression)
    {
        var statements = StatementsTree.Create(input);
        TypeChecker.CheckType(statements);
        var statementss = statements.Statements.ToList();
        Assert.Single(statementss);
        Assert.Equal(result,statementss[0].GetType());
        CheckParseIsType(((AssignmentExpression)statementss[0]).Expression, assignmentInnerExpression);
    }

    public void CheckParseIsType(GSharpExpression expression, Type type) {
        Assert.Equal(expression.GetType(), type);
    }
}