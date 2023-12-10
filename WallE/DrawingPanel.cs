using Godot;
using GSharpProject;
using GSharpProject.Evaluator;
using GSharpProject.Parsing;
using System;
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
		Font defaultFont = ThemeDB.FallbackFont;
		int defaultFontSize = ThemeDB.FallbackFontSize;
		Point p1 = new Point(600, 400, "A");
		Point p2 = new Point(700, 300, "A");
		Point p3 = new Point(600,700,"s");
		Arc arc = new Arc(p1,p3,p2,100);
		// DrawGSharpArc(arc);
		Line l1 = new Line(p1,p2);
		Line l2 = new Line(p1,p3);
		DrawGSharpSegment(l1);
		DrawGSharpSegment(l2);
		// DrawArc(new Godot.Vector2((float)p1.X,(float)p1.Y),100,0,(float)-1.57/2,200000,Colors.Black,2);
		// DrawArc(new Godot.Vector2((float)p1.X,(float)p1.Y),100,0,(float)1.57,200000,Colors.Black,2);

		DrawArc(new Godot.Vector2((float)p1.X,(float)p1.Y),100,(float)1.57,(float)-1.57/2,200000,Colors.Black,2);

		// Point r1 = new Point(0, 30, "B");
		// Point r2 = new Point(700, 150, "A");
		// Point r3 = new Point(5, 58, "C");
		// Point r4 = new Point(650, 300, "C");
		// Circle c1 = new Circle(p1, 100);
		// Circle c2 = new Circle(p2, 100);
		// //DrawGSharpPoint(p1);
		// DrawGSharpCircle(c1);
		// DrawGSharpCircle(c2);
		// List<Point> alfa = Utiles.InterceptionCircle(c1, c2);
		// foreach (Point beta in alfa)
		// 	DrawGSharpPoint(beta);

		// DrawGSharpSegment(l1);
		// DrawGSharpSegment(l2);
		// List<Point> gamma = Utiles.InterceptionLine(l1, l2);
		// foreach (Point beta in gamma)
		// 	DrawGSharpPoint(beta);
		// List<Point> interceptos = Utiles.InterceptionLine_Circle(c1, l1);
		// List<Point> interceptos1 = Utiles.InterceptionLine_Circle(c1, l2);
		// List<Point> interceptos2 = Utiles.InterceptionLine_Circle(c2, l1);
		// List<Point> interceptos3 = Utiles.InterceptionLine_Circle(c2, l2);
		// foreach (Point beta in interceptos)
		// 	DrawGSharpPoint(beta);
		// foreach (Point beta in interceptos1)
		// 	DrawGSharpPoint(beta);
		// foreach (Point beta in interceptos2)
		// 	DrawGSharpPoint(beta);
		// foreach (Point beta in interceptos3)
		// 	DrawGSharpPoint(beta);
	}
	private void DrawGSharpCircle(Circle a)
	{
		var center = new Godot.Vector2((float)a.Center.X, (float)a.Center.Y);
		DrawArc(center, (float)a.Radius, 0, (float)Math.PI * 2, 200000, Colors.Red, 2);
	}
	private void DrawGSharpPoint(Point a)
	{
		var p1 = new Godot.Vector2((float)a.X, (float)a.Y);
		DrawCircle(p1, 5, Colors.Black);
	}
	private void DrawGSharpSegment(Line a)
	{
		var start = new Godot.Vector2((float)a.StartPoint.X, (float)a.StartPoint.Y);
		var end = new Godot.Vector2((float)a.EndPoint.X, (float)a.EndPoint.Y);
		DrawLine(start, end, Colors.BlueViolet, 2);
	}

	private void DrawGSharpArc(Arc arc)
	{
		var arcCenter = new Godot.Vector2((float)arc.Center.X, (float)arc.Center.Y);
		DrawArc(arcCenter,(float)arc.Radius,arc.EndAngle,-arc.StartAngle,200000,Colors.Brown,2);
	}

	/// <summary>
	/// Runs whenever the RunButton is pressed.
	/// </summary>
	private void _on_run_button_pressed()
	{
		// Get the code edit node and access the code text.
		var codeEdit = GetNode<TextEdit>("../CodeEdit");
		var code = codeEdit.Text;

		GSharpStatementsCollection tree = StatementsTree.Create(code);

		GSharpEvaluator evaluator;
		object result;
		foreach (var i in tree.Statements)
		{
			evaluator = new GSharpEvaluator(i);
			result = evaluator.Evaluate();
			GD.Print(result);
		}
	}
}