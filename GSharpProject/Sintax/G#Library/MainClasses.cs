using System;

namespace GSharpProject;

public class Point
{
    string PointId;
    public double X;
    public double Y;

    public Type Type => typeof(Point);
    public Point(string id)
    {
        PointId = id;
        X = 100;
        Y = 100;
    }
    public Point(double x, double y, string id)
    {
        PointId = id;
        X = x;
        Y = y;
    }
}

public class Line
{
    public Line()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200,200,"b");
    }
    public Line(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public Type Type => typeof(Line);
    public Point StartPoint { get; }
    public Point EndPoint { get; }
}
public class Ray
{
    public Ray()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200,200,"b");
    }
    public Ray(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public Type Type => typeof(Ray);

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}
public class Segment
{
    public Segment()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200,200,"b");
    }
    public Segment(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public Type Type => typeof(Segment);

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}

public class Circle
{
    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }
    public Type Type => typeof(Circle);
    public Point Center { get; }
    public double Radius { get; }
}

public class Arc
{
    public Arc()
    {
        
    }
}
