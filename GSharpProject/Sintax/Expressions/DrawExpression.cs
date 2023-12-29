using System.Drawing;
using GSharpProject.Parsing;

namespace GSharpProject;

public class DrawExpression : GSharpExpression
{
    public DrawExpression(GSharpExpression argument, string message, Color color)
    {
        Argument = argument;
        Message = message;
        Color = color;
    }
    public DrawExpression(GSharpExpression argument, Color color)
    {
        Argument = argument;
        Color = color;
    }

    public override TokenType TokenType => TokenType.Draw;

    public GSharpExpression Argument { get; }
    public string Message { get; }
    public Color Color { get; }

    public override void CheckType(TypedScope typedScope)
    {
        Argument.CheckType(typedScope);
        if (Argument.ExpressionType != SingleType.Of<Sequence>() && Argument.ExpressionType != SingleType.Of<IFigure>())
        {
            throw new Exception("No se puede pintar");
        }
    }
}
