using Godot;
using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;
using System.Collections.Generic;

public partial class DrawingPanel : Panel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _Draw()
	{
		// 	DrawLine(new Vector2(0.0f, 0.0f), new Vector2(2000f, 2000f), Colors.Green, 50);
		// 	DrawRect(new Rect2(0.5f, 0.5f, 100f, 100f), Colors.Green);

		// DrawLine(new Vector2(100f, 100f), new Vector2(200, 200f), Colors.Black, 100);

		// Font defaultFont = ThemeDB.FallbackFont;
		// int defaultFontSize = ThemeDB.FallbackFontSize;
		// DrawString(defaultFont, new Vector2(200, 200), "Hello world", 0, -1, 100, Colors.Black);

		var points = new Vector2[] { new Vector2(100, 100) };
		var uvs = new Vector2[] { new Vector2(100, 100) };
		var colors = new Color[] { Colors.Black };

		// DrawPrimitive(points, colors, uvs);

		DrawCircle(new Vector2(100, 100), 5, Colors.Black);

		DrawLine(new Vector2(100, 100), new Vector2(200, 200), Colors.Black, 10);
	}

	/// <summary>
	/// Runs whenever the RunButton is pressed.
	/// </summary>
	private void _on_run_button_pressed()
	{
		// Get the code edit node and access the code text.
		var codeEdit = GetNode<TextEdit>("../CodeEdit");
		var code = codeEdit.Text;

		List<GSharpExpression> tree = StatementsTree.Create(code);

		GSharpEvaluator evaluator;
		object result;
		foreach (var i in tree)
		{
			if (i is FunctionDeclarationExpression)
			{
				continue;
			}
			evaluator = new GSharpEvaluator(i);
			result = evaluator.Evaluate();
			GD.Print(result);
		}

	}
}


