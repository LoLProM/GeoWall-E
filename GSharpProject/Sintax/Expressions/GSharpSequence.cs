using System.Collections.Generic;

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

}


