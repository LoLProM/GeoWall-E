using System.Drawing;

namespace GSharpProject;
public static class WallEColors
{
    public static Dictionary<string, Color> _Colors = new()
    {
        ["blue"]  = Color.DodgerBlue,  
        ["red"]  = Color.Red,  
        ["yellow"]  = Color.Yellow,
        ["green"] = Color.Green, 
        ["cyan"] = Color.Cyan, 
        ["magenta"] = Color.Magenta,
        ["white"] = Color.White, 
        ["gray"] = Color.Gray, 
        ["black"]   = Color.Black
    };

    public static Stack<Color>? ColorDraw;

    public static void InitializeColor()
    {
        ColorDraw = new();
        ColorDraw.Push(Color.Black);
    }
}