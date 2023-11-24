using GSharpProject;

class Point : GSharpExpression
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
}

class Line
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

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}
class Ray
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

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}
class Segment
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

    public Point StartPoint { get; }
    public Point EndPoint { get; }
}

class Circle
{
    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }

    public Point Center { get; }
    public double Radius { get; }
}
