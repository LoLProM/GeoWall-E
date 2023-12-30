using System;
using System.Collections.Generic;
using GSharpProject.Parsing;

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
            X = ran.Next(0,1000);
            Y = ran.Next(0,1000);
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
            Random ran = new();
            double lambda = 0;
            while (true)
            {
                lambda = ran.Next(int.MaxValue);
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
            Radius = ran.Next(0,400);
        }
        public Type Type => typeof(Circle);
        public Point Center { get; }
        public Measure Measure { get; }
        public double Radius { get; }
        public bool PointBelong(Point p1) => Utiles.EuclideanDistance(p1, Center) == Radius;

        public IEnumerable<Point> PointsOf()
        {
            throw new NotImplementedException();
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
            Radius = ran.Next(0,400);
            EndRay = new Point();
            StartRay = new Point();
            Center = new Point();
        }
        public bool PointBelong(Point p1) => true;
        // {
        //     Circle circle = new Circle(Center,Radius);
        // }
        public IEnumerable<Point> PointsOf()
        {
            throw new NotImplementedException();
        }
    }
    public interface IFigure
    {
        bool PointBelong(Point p1);
        IEnumerable<Point> PointsOf();

        
    }

}