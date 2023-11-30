using System.Collections.Generic;

namespace GSharpProject;

public abstract class GSharpSequence : GSharpExpression
{
}
public class GSharpLiteralSequence : GSharpSequence
{
    public GSharpLiteralSequence(Token openKey, IEnumerable<GSharpPrimitive> elements, Token closeKey) 
    {
        OpenKey = openKey;
        Elements = elements;
        CloseKey = closeKey;
    }

    public Token OpenKey { get; }
    public IEnumerable<GSharpPrimitive> Elements { get; }
    public Token CloseKey { get; }
}

public class GSharpFiniteSequence : GSharpSequence
{
    public GSharpFiniteSequence(Token openKey, IEnumerable<GSharpLiteralExpression> elements, Token closeKey) 
    {
        OpenKey = openKey;
        Elements = elements;
        CloseKey = closeKey;
    }

    public Token OpenKey { get; }
    public IEnumerable<GSharpLiteralExpression> Elements { get; }
    public Token CloseKey { get; }
}

public class GSharpInfiniteSequence : GSharpSequence
{
    public GSharpInfiniteSequence(Token openKey, IEnumerable<GSharpLiteralExpression> elements, Token closeKey) 
    {
        OpenKey = openKey;
        Elements = elements;
        CloseKey = closeKey;
    }

    public Token OpenKey { get; }
    public IEnumerable<GSharpLiteralExpression> Elements { get; }
    public Token CloseKey { get; }
}


