using System;
using System.Collections.Generic;

namespace GSharpProject;
public static class Utiles
{
    public static double EuclideanDistance(Point p1, Point p2)
    {
        double x = Math.Pow(p1.X - p2.X, 2);
        double y = Math.Pow(p1.Y - p2.Y, 2);
        return Math.Sqrt(x + y);
    }
    public static Point MiddlePoint(Point p1, Point p2)
    {
        double x = (p1.X + p2.X) / 2;
        double y = (p1.Y + p2.Y) / 2;
        return new Point(x, y);
    }
    public static List<Point> Interception(object f1, object f2)
    {
        if (f1 is Circle && f2 is Circle)
            return InterceptionCircle((Circle)f1,(Circle)f2);
        if (f1 is Circle && f2 is Line)
            return InterceptionLine_Circle((Circle)f1,(Line)f2);
        if (f1 is Line && f2 is Line)
            return InterceptionLine((Line)f1,(Line)f2);
        if (f1 is Segment && f2 is Segment)
            return InterceptionSegment((Segment)f1,(Segment)f2);
        if (f1 is Segment && f2 is Circle)
            return InterceptionSegment_Circle((Segment)f1,(Circle)f2);
        if (f1 is Segment && f2 is Line)
            return InterceptionSegment_Line((Segment)f1,(Line)f2);
        if (f1 is Arc && f2 is Arc)
            return InterceptionArc((Arc)f1,(Arc)f2);
        if (f1 is Arc && f2 is Circle)
            return InterceptionArc_Circle((Arc)f1,(Circle)f2);
        if (f1 is Arc && f2 is Segment)
            return InterceptionArc_Segment((Arc)f1,(Segment)f2);
        if (f1 is Arc && f2 is Line)
            return InterceptionArc_Line((Arc)f1,(Line)f2);
        if (f1 is Ray && f2 is Ray)
            return InterceptionRay((Ray)f1,(Ray)f2);
        if (f1 is Ray && f2 is Line)
            return InterceptionRay_Line((Ray)f1,(Line)f2);
        if (f1 is Ray && f2 is Circle)
            return InterceptionRay_Circle((Ray)f1,(Circle)f2);
        if (f1 is Ray && f2 is Segment)
            return InterceptionRay_Segment((Ray)f1,(Segment)f2);
        if (f1 is Ray && f2 is Circle)
            return InterceptionRay_Circle((Ray)f1,(Circle)f2);
            
        return new List<Point>();
    }

    public static List<Point> InterceptionCircle(Circle c1, Circle c2)
    {
        List<Point> intercept = new List<Point>();
        double centerDistance = EuclideanDistance(c1.Center, c2.Center);
        if (centerDistance > c1.Radius + c2.Radius)
            return intercept;
        else if (centerDistance == 0)
            return intercept;
        else if (c1.Radius == c2.Radius && c1.Center.PointBelong(c2.Center))
            return null!;
        Point middle = MiddlePoint(c1.Center, c2.Center);

        double a = (Math.Pow(c1.Radius, 2) - Math.Pow(c2.Radius, 2) + Math.Pow(centerDistance, 2)) / (2 * centerDistance);
        double h = Math.Sqrt(Math.Pow(c1.Radius, 2) - Math.Pow(a, 2));

        double x_intercept1 = middle.X + h * (c2.Center.Y - c1.Center.Y) / centerDistance;
        double y_intercept1 = middle.Y - h * (c2.Center.X - c1.Center.X) / centerDistance;
        double x_intercept2 = middle.X - h * (c2.Center.Y - c1.Center.Y) / centerDistance;
        double y_intercept2 = middle.Y + h * (c2.Center.X - c1.Center.X) / centerDistance;

        Point inter_1 = new Point(x_intercept1, y_intercept1);
        Point inter_2 = new Point(x_intercept2, y_intercept2);

        intercept.Add(inter_1);
        if (inter_1.X != inter_2.X || inter_1.Y != inter_2.Y)
            intercept.Add(inter_2);

        return intercept;
    }
    public static List<Point> InterceptionLine_Circle(Circle c1, Line l1)
    {
        List<Point> intercept = new List<Point>();

        double a = c1.Center.X;
        double b = c1.Center.Y;
        double r = c1.Radius;

        if ((l1.StartPoint.X - l1.EndPoint.X) != 0)
        {
            double m = (l1.StartPoint.Y - l1.EndPoint.Y) / (l1.StartPoint.X - l1.EndPoint.X);
            double n = l1.StartPoint.Y - m * l1.StartPoint.X;

            if (Math.Pow((2 * m * n - 2 * b * m - 2 * a), 2) - 4 * (m * m + 1) * (a * a + n * n - 2 * b * n + b * b - r * r) < 0) return intercept;

            double interceptX1 = (-(2 * m * n - 2 * b * m - 2 * a) - Math.Sqrt(Math.Pow((2 * m * n - 2 * b * m - 2 * a), 2) - 4 * (m * m + 1) * (a * a + n * n - 2 * b * n + b * b - r * r))) / (2 * (m * m + 1));
            double interceptX2 = (-(2 * m * n - 2 * b * m - 2 * a) + Math.Sqrt(Math.Pow((2 * m * n - 2 * b * m - 2 * a), 2) - 4 * (m * m + 1) * (a * a + n * n - 2 * b * n + b * b - r * r))) / (2 * (m * m + 1));

            double interceptY1 = m * interceptX1 + n;
            double interceptY2 = m * interceptX2 + n;
            Point p1 = new Point(interceptX1, interceptY1);
            intercept.Add(p1);
            if (!p1.PointBelong(new Point(interceptX2, interceptY2))) intercept.Add(new Point(interceptX2, interceptY2));
            return intercept;
        }
        else
        {
            double x = l1.StartPoint.X;
            if (r * r - (x - a) * (x - a) < 0) return intercept;
            double y1 = b - Math.Sqrt(r * r - (x - a) * (x - a));
            double y2 = b + Math.Sqrt(r * r - (x - a) * (x - a));
            intercept.Add(new Point(x, y1));
            if (y1 != y2) intercept.Add(new Point(x, y2));
        }
        return intercept;
    }

    public static List<Point> InterceptionLine(Line l1, Line l2)
    {
        double a = l1.StartPoint.X;
        double b = l1.StartPoint.Y;
        double A = l1.EndPoint.X - a;
        double B = l1.EndPoint.Y - b;
        double c = l2.StartPoint.X;
        double d = l2.StartPoint.Y;
        double C = l2.EndPoint.X - c;
        double D = l2.EndPoint.Y - d;

        double direction = (l1.StartPoint.X - l1.EndPoint.X) * (l2.StartPoint.Y - l2.EndPoint.Y) -
        (l1.StartPoint.Y - l1.EndPoint.Y) * (l2.StartPoint.X - l2.EndPoint.X);

        if (direction == 0)
        {
            if (l1.PointBelong(l2.EndPoint))
                return null;//Modificar cuando se implemente Pointsof
            return new List<Point>();
        }
        double lambda = ((c - a) * D + (b - d) * C) / (D * A - B * C);
        double xintercept = a + lambda * A;
        double yintercept = b + lambda * B;
        return new List<Point>() { new Point(xintercept, yintercept) };
    }
    public static List<Point> InterceptionSegment(Segment s1, Segment s2)
    {
        var pointList = new List<Point>();
        Line line = new Line(s1.StartPoint, s1.EndPoint);
        Line secondLine = new Line(s2.StartPoint, s2.EndPoint);

        var posibleInterception = InterceptionLine(line, secondLine);

        foreach (var interception in posibleInterception)
        {
            if (s2.PointBelong(interception) && s1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }
        return pointList;
    }

    public static List<Point> InterceptionSegment_Circle(Segment s1, Circle c2)
    {
        var pointList = new List<Point>();
        Line line = new Line(s1.StartPoint, s1.EndPoint);

        var posibleInterception = InterceptionLine_Circle(c2, line);

        foreach (var interception in posibleInterception)
        {
            if (s1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }
        return pointList;
    }

    public static List<Point> InterceptionSegment_Line(Segment s1, Line l2)
    {
        var pointList = new List<Point>();
        Line line = new Line(s1.StartPoint, s1.EndPoint);

        var posibleInterception = InterceptionLine(line, l2);

        foreach (var interception in posibleInterception)
        {
            if (s1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }
        return pointList;
    }

    public static List<Point> InterceptionArc(Arc a1, Arc a2)
    {
        var pointList = new List<Point>();
        Circle c1 = new Circle(a1.Center, new Measure(a1.Radius));
        Circle c2 = new Circle(a2.Center, new Measure(a1.Radius));

        var posibleInterception = InterceptionCircle(c1, c2);

        foreach (var interception in posibleInterception)
        {
            if (a1.PointBelong(interception) && a2.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionArc_Circle(Arc a1, Circle c2)
    {
        var pointList = new List<Point>();
        Circle c1 = new Circle(a1.Center, new Measure(a1.Radius));

        var posibleInterception = InterceptionCircle(c1, c2);

        foreach (var interception in posibleInterception)
        {
            if (a1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionArc_Segment(Arc a1, Segment s2)
    {
        var pointList = new List<Point>();
        Circle c1 = new Circle(a1.Center, new Measure(a1.Radius));
        Line l1 = new Line(s2.StartPoint, s2.EndPoint);

        var posibleInterception = InterceptionLine_Circle(c1, l1);

        foreach (var interception in posibleInterception)
        {
            if (a1.PointBelong(interception) && s2.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionArc_Line(Arc a1, Line l2)
    {
        var pointList = new List<Point>();
        Circle c1 = new Circle(a1.Center, new Measure(a1.Radius));

        var posibleInterception = InterceptionLine_Circle(c1, l2);

        foreach (var interception in posibleInterception)
        {
            if (a1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionRay(Ray r1, Ray r2)
    {
        var pointList = new List<Point>();
        Line l1 = new Line(r1.StartPoint, r1.EndPoint);
        Line l2 = new Line(r2.StartPoint, r2.EndPoint);

        var posibleInterception = InterceptionLine(l1, l2);

        foreach (var interception in posibleInterception)
        {
            if (r1.PointBelong(interception) && r2.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionRay_Line(Ray r1, Line l2)
    {
        var pointList = new List<Point>();
        Line l1 = new Line(r1.StartPoint, r1.EndPoint);

        var posibleInterception = InterceptionLine(l1, l2);

        foreach (var interception in posibleInterception)
        {
            if (r1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }

        return pointList;
    }

    public static List<Point> InterceptionRay_Segment(Ray r1, Segment s2)
    {
        var pointList = new List<Point>();
        Line l1 = new Line(r1.StartPoint, r1.EndPoint);

        var posibleInterception = InterceptionSegment_Line(s2, l1);

        foreach (var interception in posibleInterception)
        {
            if (r1.PointBelong(interception) && s2.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }
        return pointList;
    }

    public static List<Point> InterceptionRay_Circle(Ray r1, Circle c2)
    {
        var pointList = new List<Point>();
        Line l1 = new Line(r1.StartPoint, r1.EndPoint);

        var posibleInterception = InterceptionLine_Circle(c2, l1);

        foreach (var interception in posibleInterception)
        {
            if (r1.PointBelong(interception))
            {
                pointList.Add(interception);
            }
        }
        return pointList;
    }

    internal static IEnumerator<Point> GetSamples()
    {
        while (true)
            yield return new Point();
    }

    internal static IEnumerable<Point> GetPointsOf(object v)
    {
        return ((IFigure)v).PointsOf();
    }

    internal static IEnumerator<double> GetRandom()
    {
        Random a = new();
        while (true)
            yield return a.NextDouble();
    }

    internal static int? GetCountSequence(Sequence v)
    {
        return v.Count;
    }
}
