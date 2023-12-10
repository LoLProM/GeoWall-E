using System.Collections.Generic;

namespace GSharpProject;

public class GSharpRayExpression : GSharpPrimitive
{
    public GSharpRayExpression(Token ray, string identifier)
    {
        Ray = ray;
        Identifier = identifier;
        ExpressionType = typeof(Line);
    }

    public GSharpRayExpression(Token ray, List<string> coordinates)
    {
        Ray = ray;
        Coordinates = coordinates;
        ExpressionType = typeof(Line);
    }

    public Token Ray { get; }
    public List<string> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Ray;

}