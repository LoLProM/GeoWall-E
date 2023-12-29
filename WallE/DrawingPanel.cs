using Godot;
using System.Collections.Generic;
using GSharpProject;
using GSharpProject.Parsing;
using System;
using GSharpProject.Evaluator;
using System.Linq;

public partial class DrawingPanel : Panel
{
	public List<(object,Color,string)> figures;
	public bool draw = false;
	Font defaultFont = ThemeDB.FallbackFont;
	int defaultFontSize = ThemeDB.FallbackFontSize;
	bool vector_declared;
	Vector2 traslation_vector;
	bool found;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Draw();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _Draw()
	{
		vector_declared = false;
		Vector2 traslation_vector = new Vector2(0, 0);
		string text;
		found = false;

		foreach (var (figure,color,message) in figures)
		{
			if(figure is Arc arc)
			{
				DrawGSharpArc(arc,color,message);
			}
			else if (figure is LiteralSequence sequenceOf)
			{
				
			}
			else if(figure is Point point)
			{
				DrawGSharpPoint(point,color,message);
			}
			else if(figure is Circle circle)
			{
				DrawGSharpCircle(circle,color,message);
				
			}
			else if(figure is Line line)
			{
				DrawGSharpLine(line,color,message);
				
			}
			else if(figure is Ray ray)
			{
				DrawGSharpRay(ray,color,message);
				
			}
			else if(figure is Segment segment)
			{
				DrawGSharpSegment(segment,color,message);
			}
		}
	}
    private void DrawGSharpLine(Line a, Godot.Color color, string msg = "")
	{
		var startPoint = new Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
		var endPoint = new Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);

		Godot.Vector2 vectorDirector = (endPoint - startPoint).Normalized();
		Godot.Vector2 start = startPoint - vectorDirector * 1000;
		Godot.Vector2 end = endPoint + vectorDirector * 1000;
		DrawLine(start, end, color, 2, true);

	}
	private void DrawGSharpRay(Ray a, Godot.Color color, string msg = "")
	{
		var startPoint = new Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
		var endPoint = new Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);

		Godot.Vector2 vectorDirector = (endPoint - startPoint).Normalized();
		Godot.Vector2 end = endPoint + vectorDirector * 1000;
		DrawLine(startPoint, end, color, 2, true);

	}
	private void DrawGSharpCircle(Circle a, Godot.Color color, string msg = "")
	{
		var center = new Godot.Vector2((float)a.Center.X, (float)a.Center.Y);
		DrawArc(center, (float)a.Radius, 0, (float)Math.PI * 2, 20000, color, 2, true);
	}
	private void DrawGSharpPoint(Point a, Godot.Color color, string msg = "")
	{
		var p1 = new Godot.Vector2((float)a.X, (float)a.Y);
		DrawCircle(p1, 5, color);
	}
	private void DrawGSharpSegment(Segment a, Godot.Color color, string msg = "")
	{
		var start = new Godot.Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
		var end = new Godot.Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);
		DrawLine(start, end, color, 2, true);
	}

	private void DrawGSharpArc(Arc arc, Godot.Color color, string msg = "")
	{
		var arcCenter = new Godot.Vector2((float)arc.Center.X, (float)arc.Center.Y);
		var startPoint = new Godot.Vector2((float)arc.StartRay.X, (float)arc.StartRay.Y);
		var endPoint = new Godot.Vector2((float)arc.EndRay.X, (float)arc.EndRay.Y);
		var startAngle = (startPoint - arcCenter).Normalized().Angle();
		var endAngle = (endPoint - arcCenter).Normalized().Angle();
		float angleDifference = endAngle - startAngle;

		if (angleDifference < 0)
		{
			angleDifference += Mathf.Pi * 2;
		}

		DrawArc(arcCenter, (float)arc.Radius, startAngle, startAngle + angleDifference, 20000, color, 2, true);
	}
	
	/// <summary>
	/// Runs whenever the RunButton is pressed.
	/// </summary>
	private void _on_run_button_pressed()
	{
		// Get the code edit node and access the code text.
		var codeEdit = GetNode<TextEdit>("../CodeEdit");
		var code = codeEdit.Text;
		// StandardLibrary.Initialize();
        // var statements = StatementsTree.Create(code);
        // TypeChecker.CheckType(statements);
        // var evaluator = new GSharpEvaluator(statements);
        // var listOfDrawable = evaluator.Evaluate();
		// figures = listOfDrawable;
		// draw = true;
	}
}


