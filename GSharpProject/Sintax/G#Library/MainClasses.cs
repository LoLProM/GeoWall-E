using System;
using System.Collections.Generic;
using System.Numerics;
using GSharpProject.Parsing;
#region Espacio determinado para todos los tipos de objetos como tal del programa puntos lineas segmentos rayos circulos arcos
namespace GSharpProject
{
    public class Point : IFigure
    {
        public double X;
        public double Y;

        public Type Type => typeof(Point);

        public Point()
        {
            Random ran = new Random();
            X = ran.Next(0, 1000);
            Y = ran.Next(0, 1000);
        }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool PointBelong(Point p1) => (X == p1.X) && (Y == p1.Y);

        public IEnumerable<Point> PointsOf()
        {
            yield return new Point(X, Y);
        }
    }

    public class Line : IFigure
    {
        public Line()
        {
            StartPoint = new Point();
            EndPoint = new Point();
        }
        public Line(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
        public Type Type => typeof(Line);
        public Point StartPoint { get; }
        public Point EndPoint { get; }

        public bool PointBelong(Point p1)
        {
            double dx = EndPoint.X - StartPoint.X;
            double dy = EndPoint.Y - StartPoint.Y;

            if (dx == 0) return (p1.Y == EndPoint.Y);
            else if (dy == 0) return (p1.X == EndPoint.X);
            double new_dx = (EndPoint.X - p1.X);
            double new_dy = (EndPoint.Y - p1.Y);
            return Math.Abs((new_dy / new_dx) - (dy / dx)) <= 0.0001;
        }

        public IEnumerable<Point> PointsOf()
        {
            //Lambda real
            Random ran = new Random();
            double lambda = 0;
            while (true)
            {
                lambda = ran.Next(-100, 100);
                double x = StartPoint.X + lambda * (StartPoint.X - EndPoint.X);
                double y = StartPoint.Y + lambda * (StartPoint.Y - EndPoint.Y);
                yield return new Point(x, y);
            }

        }
    }
    public class Ray : IFigure
    {
        public Ray()
        {
            StartPoint = new Point();
            EndPoint = new Point();
        }
        public Ray(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
        public bool PointBelong(Point p1)
        {
            Line l = new Line(StartPoint, EndPoint);
            if (l.PointBelong(p1))
            {

                if (StartPoint.X <= EndPoint.X)
                {
                    if (p1.X >= StartPoint.X)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    if (p1.X <= StartPoint.X)
                    {
                        return true;
                    }
                    return false;
                }

            }
            return false;
        }
        public IEnumerable<Point> PointsOf()
        {
            Random ran = new();
            double lambda = 0;
            while (true)
            {
                lambda += 1;
                double x = StartPoint.X + lambda * (EndPoint.X - StartPoint.X);
                double y = StartPoint.Y + lambda * (EndPoint.Y - StartPoint.Y);
                yield return new Point(x, y);
            }
        }
        public Type Type => typeof(Ray);
        public Point StartPoint { get; }
        public Point EndPoint { get; }
    }
    public class Segment : IFigure
    {
        public Segment()
        {
            StartPoint = new Point();
            EndPoint = new Point();
        }
        public Segment(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
        public bool PointBelong(Point p1) => Math.Abs(Utiles.EuclideanDistance(p1, StartPoint) + Utiles.EuclideanDistance(p1, EndPoint) - Utiles.EuclideanDistance(StartPoint, EndPoint)) <= 0.0001;

        public IEnumerable<Point> PointsOf()
        {
            Random ran = new();
            double lambda = 0;
            while (true)
            {
                //lambda entre cero y uno
                lambda = ran.NextDouble();
                double x = StartPoint.X + lambda * (EndPoint.X - StartPoint.X);
                double y = StartPoint.Y + lambda * (EndPoint.Y - StartPoint.Y);
                yield return new Point(x, y);
            }
        }

        public Type Type => typeof(Segment);

        public Point StartPoint { get; }
        public Point EndPoint { get; }
    }

    public class Circle : IFigure
    {
        public Circle(Point center, Measure measure)
        {
            Center = center;
            Measure = measure;
            Radius = measure.EuclideanDistance;
        }
        public Circle()
        {
            Center = new Point();
            Random ran = new Random();
            Radius = ran.Next(0, 400);
        }
        public Type Type => typeof(Circle);
        public Point Center { get; }
        public Measure Measure { get; }
        public double Radius { get; }
        public bool PointBelong(Point p1) => Math.Abs(Utiles.EuclideanDistance(p1, Center) - Radius) < 0.0001;

        public IEnumerable<Point> PointsOf()
        {
            Random ran = new Random();
            for (double a = 0; a <= Math.PI * 2; a += ran.NextDouble())
            {
                double x = Center.X + Radius * Math.Cos(a);
                double y = Center.Y + Radius * Math.Sin(a);
                yield return new Point(x, y);
            }
        }
    }

    public class Arc : IFigure
    {
        public Point Center { get; }
        public Point StartRay { get; }
        public Point EndRay { get; }
        public Measure Measure { get; }
        public double Radius { get; }
        public Type Type => typeof(Arc);

        public Arc(Point center, Point startRay, Point endRay, Measure measure)
        {
            Radius = measure.EuclideanDistance;
            EndRay = endRay;
            Measure = measure;
            StartRay = startRay;
            Center = center;
        }
        public Arc()
        {
            Random ran = new Random();
            Radius = ran.Next(0, 400);
            EndRay = new Point();
            StartRay = new Point();
            Center = new Point();
        }
        public bool PointBelong(Point p1)
        {
            Circle a = new Circle(Center, new Measure(Radius));
            if (a.PointBelong(p1))
            {
                double angleP1 = Math.Atan2(p1.Y, p1.X);
                double start = Math.Atan2(StartRay.Y, StartRay.X);
                double end = Math.Atan2(EndRay.Y, EndRay.X);
                if (angleP1 < 0) angleP1 += 2 * Math.PI;
                if (start < 0) start += 2 * Math.PI;
                if (end < 0) end += 2 * Math.PI;
                if (start <= end)
                {
                    return angleP1 >= start && angleP1 <= end;
                }
                else
                {
                    return angleP1 >= end || angleP1 <= start;
                }
            }
            return false;

        }

        public IEnumerable<Point> PointsOf()
        {
            Random ran = new Random();

            double start = Math.Atan2(StartRay.X, StartRay.Y);
            double end = Math.Atan2(EndRay.X, EndRay.Y);
            if (start < 0) start += 2 * Math.PI;
            if (end < 0) end += 2 * Math.PI;
            if (start <= end)
            {
                for (double a = start; a <= end; a += ran.NextDouble())
                {
                    double x = Center.X + Radius * Math.Cos(a);
                    double y = Center.Y + Radius * Math.Sin(a);
                    yield return new Point(x, y);
                }
            }
            else
            {
                for (double a = 0, b = end; a <= start || b >= end; a += ran.NextDouble(), b -= ran.NextDouble())
                {
                    double x = Center.X + Radius * Math.Cos(a);
                    double y = Center.Y + Radius * Math.Sin(a);
                    double x1 = Center.X + Radius * Math.Cos(b);
                    double y1 = Center.Y + Radius * Math.Sin(b);
                    if (a <= start)
                        yield return new Point(x, y);
                    if (b >= end)
                        yield return new Point(x1, y1);
                }


            }

        }
    }
    public interface IFigure
    {
        bool PointBelong(Point p1);
        IEnumerable<Point> PointsOf();
    }
    #endregion
}