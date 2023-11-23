using GSharpProject;

class Point : GSharpExpression
{
    string PointId;

    public int X;
    public int Y;
    public Point(string id)
    {
        PointId = id;
        X = 100;
        Y = 100;
    }

    public Point(int x, int y, string id)
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

class Circle
{
    public Circle(Point center, int radius)
    {
        Center = center;
        Radius = radius;
    }

    public Point Center { get; }
    public int Radius { get; }
}