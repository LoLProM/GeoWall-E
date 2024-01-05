using System.Drawing;
using GSharpProject.Parsing;

namespace GSharpProject;
public static class WallEColors
{
    public static Dictionary<string, Color> _Colors = new()
    {
        ["blue"] = Color.DodgerBlue,
        ["red"] = Color.Red,
        ["yellow"] = Color.Yellow,
        ["green"] = Color.Green,
        ["cyan"] = Color.Cyan,
        ["magenta"] = Color.Magenta,
        ["white"] = Color.White,
        ["gray"] = Color.Gray,
        ["black"] = Color.Black
    };

    public static Stack<Color>? ColorDraw;
    public static void InitializeColor()
    {
        ColorDraw = new();
        ColorDraw.Push(Color.Black);
    }


}
public class ColorExpression : GSharpExpression
{
    public ColorExpression(string color)
    {
        Color = color;
    }
    public override TokenType TokenType => TokenType.ColorExpression;

    public string Color { get; }

    public override void CheckType(TypedScope typedScope)
    {
        ExpressionType = SingleType.Of<Color>();
    }
}