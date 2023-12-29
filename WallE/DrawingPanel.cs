using Godot;
using System.Collections.Generic;
using GSharpProject;
using GSharpProject.Parsing;
using System;

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
		// Font defaultFont = ThemeDB.FallbackFont;
		// int defaultFontSize = ThemeDB.FallbackFontSize;
		// Point p1 = new Point(200, 300);
		// Point p2 = new Point(200, 500);
		// Point p3 = new Point(500, 300);
		// Point p4 = new Point(500, 500);
		// Point p5 = new Point(350, 100);

		// DrawGSharpPoint(p1, Colors.Black);
		// //DrawGSharpPoint(p2);
		// DrawGSharpPoint(p3, Colors.Black);
		// DrawGSharpRay(new Ray(p1, p3), Colors.Cyan);
		// DrawGSharpRay(new Ray(p1, p2), Colors.Gold);
		// DrawGSharpRay(new Ray(p1, p5), Colors.OrangeRed);
		// Arc alfa = new Arc(p1, p2, p5, new Measure(100));
		// Arc beta = new Arc(p1, p5, p2, new Measure(100));
		// Arc gamma = new Arc(p1, p3, p2, new Measure(150));
		// Arc delta = new Arc(p1, p2, p3, new Measure(150));

		// DrawGSharpArc(beta, Colors.Olive);
		// DrawGSharpArc(alfa, Colors.Aqua);
		// DrawGSharpArc(gamma, Colors.GreenYellow);
		// DrawGSharpArc(delta, Colors.DarkMagenta);
		// DrawGSharpPoint(p1, Colors.Black);
		// //DrawGSharpPoint(p2);
		// DrawGSharpPoint(p3, Colors.Black);
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
	}
}


