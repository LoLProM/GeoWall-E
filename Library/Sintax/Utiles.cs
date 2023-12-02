using System;
using System.Collections.Generic;

namespace GSharpProject;

public static class Utiles
{
    public static double EuclideanDistance(Point p1, Point p2)
    {
        double x = Math.Pow((p1.X - p2.X), 2);
        double y = Math.Pow((p1.Y - p2.Y), 2);
        return Math.Sqrt(x + y);
    }
    public static Point MiddlePoint(Point p1, Point p2)
    {
        double x = (p1.X + p2.X) / 2;
        double y = (p1.Y + p2.Y) / 2;
        return new Point(x, y, "Mid");
    }
    public static List<Point> Interception(Object f1, Object f2)
    {
        Type _f1 = f1.GetType();
        Type _f2 = f2.GetType();

        switch (_fi)
        {
            case Circle when _f2 == typeof(Circle):
                return InterceptionCircle((Circle)f1, (Circle)f2);
            case Circle when _f2 == typeof(Line):
                return InterceptionLine_Circle((Circle)f1, (Line)f2);
            case Line when _f2 == typeof(Circle):
                return InterceptionLine_Circle((Circle)f2, (Line)f1);
            case Line when _f2 == typeof(Line):
                return InterceptionLine((Line)f2, (Line)f1);
            default:
                return new List<Point>();
        }
    }

    public static List<Point> InterceptionCircle(Circle c1, Circle c2)
    {
        List<Point> intercept = new List<Point>();
        double centerDistance = EuclideanDistance(c1.Center, c2.Center);
        if (centerDistance > c1.Radius + c2.Radius)
            return intercept;
        else if (centerDistance == 0)
            return intercept;

        double a = (Math.Pow(c1.Radius, 2) - Math.Pow(c2.Radius, 2) + Math.Pow(centerDistance, 2)) / (2 * centerDistance);
        double h = Math.Sqrt(Math.Pow(c1.Radius, 2) - Math.Pow(a, 2));


        Point middle = MiddlePoint(c1.Center, c2.Center);

        double x_intercept1 = middle.X + h * (c2.Center.X - c1.Center.X) / centerDistance;
        double y_intercept1 = middle.y - h * (c2.Center.X - c1.Center.X) / centerDistance;
        double x_intercept2 = middle.X - h * (c2.Center.X - c1.Center.X) / centerDistance;
        double y_intercept2 = middle.X + h * (c2.Center.X - c1.Center.X) / centerDistance;

        Point inter_1 = new Point(x_intercept1, y_intercept1, "a1");
        Point inter_2 = new Point(x_intercept2, y_intercept2, "a2");

        intercept.Add(inter_1);
        if (inter_1.X != inter_2.X || inter_1.Y != inter_2.Y)
            intercept.Add(inter_2);
        return intercept;
    }
    public static List<Point> InterceptionLine_Circle(Circle c1, Line l1)
    {
        throw new NotImplementedException();
    }
    public static List<Point> InterceptionLine(Line l1, Line l2)
    {
        double direction = (l1.StartPoint.X - l1.EndPoint.X) * (l2.StartPoint.Y - l2.EndPoint.Y) -
        (l1.StartPoint.Y - l1.EndPoint.Y) * (l2.StartPoint.X - l2.EndPoint.X);

        if (direction == 0)
        {
            if (Contiene(l1, l2.StartPoint))
                return new List<Point>();//Modificar cuando se implemente Pointsof
            return new List<Point>();
        }
        return new List<Point>();
    }
    public static bool Contiene(Line l1, Point p1)
    {
        return true;
    }

}
