<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
using GSharpProject;

class Point : GSharpExpression
=======
using System;
using System.Collections.Generic;

namespace GSharpProject;

public class Point : GSharpExpression, IFigure
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
{
    string PointId;

    public double X;
    public double Y;
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

    public bool PointBelong(Point p1) => (X == p1.X) && (Y == p1.Y);

    public IEnumerable<Point> PointsOf()
    {
        yield return new Point(X, Y, PointId);
    }
}

<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
class Line
=======
public class Line : IFigure
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
{
    public Line()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200, 200, "b");
    }
    public Line(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs

=======
    public Type Type => typeof(Line);
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
    public Point StartPoint { get; }
    public Point EndPoint { get; }

    public bool PointBelong(Point p1)
    {
        double dx = (EndPoint.X - StartPoint.X);
        double dy = (EndPoint.Y - StartPoint.Y);

        if (dx == 0) return (p1.Y == EndPoint.Y);
        else if (dy == 0) return (p1.X == EndPoint.X);
        double new_dx = (EndPoint.X - p1.X);
        double new_dy = (EndPoint.Y - p1.Y);
        return (new_dy / new_dx) == (dy / dx);
    }

    public IEnumerable<Point> PointsOf()
    {
        //Lambda real
        double lambda = 0;
        while (true)
        {
            lambda++;
            double x = StartPoint.X + lambda * (StartPoint.X - EndPoint.X);
            double y = StartPoint.Y + lambda * (StartPoint.Y - EndPoint.Y);
            yield return new Point(x, y, "A");
        }

    }
}
<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
class Ray
=======
public class Ray : IFigure
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
{
    public Ray()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200, 200, "b");
    }
    public Ray(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
    public bool PointBelong(Point p1)
    {
        Line l1 = new Line(StartPoint, EndPoint);
        if (!l1.PointBelong(p1)) return false;
        double dx = (EndPoint.X - StartPoint.X);
        double dy = (EndPoint.Y - StartPoint.Y);

        switch (dx)
        {
            case 0:
                if (EndPoint.Y > StartPoint.Y) return p1.Y >= StartPoint.Y;
                return p1.Y <= StartPoint.Y;
            case < 0:
                return p1.X <= StartPoint.X;
            default:
                return p1.X >= StartPoint.X;
        }

    }

    public IEnumerable<Point> PointsOf()
    {
        double lambda = 0;
        while (true)
        {
            //lambda no negativo
            lambda++;
            double x = StartPoint.X + lambda * (EndPoint.X - StartPoint.X);
            double y = StartPoint.Y + lambda * (EndPoint.Y - StartPoint.Y);
            yield return new Point(x, y, "A");
        }
    }

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}
<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
class Segment
=======
public class Segment : IFigure
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
{
    public Segment()
    {
        StartPoint = new Point("a");
        EndPoint = new Point(200, 200, "b");
    }
    public Segment(Point startPoint, Point endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
    public bool PointBelong(Point p1) => Utiles.EuclideanDistance(p1, StartPoint) + Utiles.EuclideanDistance(p1, EndPoint) == Utiles.EuclideanDistance(StartPoint, EndPoint);

    public IEnumerable<Point> PointsOf()
    {
        double lambda = 0;
        while (true)
        {
            //lambda entre cero y uno
            lambda += 0.1;
            double x = StartPoint.X + lambda * (EndPoint.X - StartPoint.X);
            double y = StartPoint.Y + lambda * (EndPoint.Y - StartPoint.Y);
            yield return new Point(x, y, "A");
        }
    }

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}

<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
class Circle
=======
public class Circle : IFigure
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
{
    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    public Point Center { get; }
    public double Radius { get; }
<<<<<<< Updated upstream:Library/G#Library/MainClasses.cs
=======
    public bool PointBelong(Point p1) => Utiles.EuclideanDistance(p1, Center) == Radius;

    public IEnumerable<Point> PointsOf()
    {

    }
}



public class Arc : IFigure
{
    public Point Center { get; }
    public Point StartRay { get; }
    public Point EndRay { get; }
    public double Measure { get; }
    public Type Type => typeof(Arc);

    public Arc(Point center, Point startRay, Point endRay, double measure)
    {
        Measure = measure;
        EndRay = endRay;
        StartRay = startRay;
        Center = center;
    }
    public bool PointBelong(Point p1) => true;

    public IEnumerable<Point> PointsOf()
    {
        throw new NotImplementedException();
    }
}
public enum Color
{
    blue,
    red,
    green,
    cyan,
    magenta,
    white,
    gray,
    black
}
public interface IFigure
{
    bool PointBelong(Point p1);
    IEnumerable<Point> PointsOf();
>>>>>>> Stashed changes:Library/Sintax/G#Library/MainClasses.cs
}
