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

        if (Argument.ExpressionType is CompoundType compoundType && !compoundType.ContentType.Type.GetInterfaces().Contains(typeof(IFigure)) || Argument.ExpressionType is SingleType singleType && !singleType.Type.GetInterfaces().Contains(typeof(IFigure)))
        {
            throw new Exception("no se puede pintar");
        }

    }
}
