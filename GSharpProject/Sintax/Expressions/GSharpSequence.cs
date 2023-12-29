using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public abstract class GSharpSequence : GSharpExpression
{
}
public class GSharpLiteralSequence : GSharpSequence
{
    public GSharpLiteralSequence(Token openKey, GSharpExpression[] elements, Token closeKey)
    {
        OpenKey = openKey;
        Elements = elements;
        CloseKey = closeKey;
    }

    public Token OpenKey { get; }
    public GSharpExpression[] Elements { get; }
    public Token CloseKey { get; }

    public override TokenType TokenType => TokenType.LiteralSequence;

    public override void CheckType(TypedScope typedScope)
    {
        if(HasSameTypeInAllElement(typedScope))
        {
            ExpressionType = new CompoundType(typeof(LiteralSequence),Elements[0].ExpressionType!);
        }
    }
    private bool HasSameTypeInAllElement(TypedScope typedScope)
    {
        foreach (var element in Elements)
        {
            element.CheckType(typedScope);
        }
        var firstType = Elements[0].ExpressionType;
        return Elements.All(e => e.ExpressionType == firstType);
    }
}

public class GSharpRangeSequence : GSharpSequence
{
    public GSharpRangeSequence(Token openKey, GSharpLiteralExpression first, GSharpLiteralExpression last, Token closeKey)
    {
        OpenKey = openKey;
        First = first;
        Last = last;
        CloseKey = closeKey;
    }

    public Token OpenKey { get; }
    public GSharpLiteralExpression First { get; }
    public GSharpLiteralExpression Last { get; }
    public Token CloseKey { get; }
    public override TokenType TokenType => TokenType.RangeSequence;

    public override void CheckType(TypedScope typedScope)
    {
        First.CheckType(typedScope);
        Last.CheckType(typedScope);
        if (First.ExpressionType == Last.ExpressionType && First.ExpressionType == SingleType.Of<int>())
        {
            ExpressionType = new CompoundType(typeof(Sequence),new SingleType(typeof(int)));
        }
    }
}

public class GSharpInfiniteSequence : GSharpSequence
{
    public GSharpInfiniteSequence(Token openKey, GSharpLiteralExpression first, Token closeKey)
    {
        OpenKey = openKey;
        First = first;
        CloseKey = closeKey;
    }
    public Token OpenKey { get; }
    public GSharpLiteralExpression First { get; }
    public Token CloseKey { get; }
    public override TokenType TokenType => TokenType.InfiniteSequence;

    public override void CheckType(TypedScope typedScope)
    {
        First.CheckType(typedScope);
        if (First.ExpressionType == SingleType.Of<int>())
        {
            ExpressionType = new CompoundType(typeof(Sequence),new SingleType(typeof(int)));
        }
    }
}


