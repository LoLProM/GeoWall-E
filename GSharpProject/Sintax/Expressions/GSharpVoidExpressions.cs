using GSharpProject;
using GSharpProject.Parsing;

public class GSharpVoidEx : GSharpExpression
{
    public override TokenType TokenType => TokenType.VoidExpression;

    public override void CheckType(TypedScope typedScope)
    {
    }
}