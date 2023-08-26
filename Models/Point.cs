namespace ClosedEllipse.Models;

public record Point
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double Z { get; private set; }

    public double Length { get; private set; }
    public static readonly Point ZERO = new(0, 0, 0);


    public Point(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;

        Length = Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public static Point operator -(Point p1, Point p2) { return new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z); }

    public static double Distance(Point p1, Point p2) { return (p2 - p1).Length; }
    public static double Distance(Point p) { return p.Length; }
}