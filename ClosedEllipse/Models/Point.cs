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

    public Point(double[] coordinates)
    {
        if (coordinates == null || coordinates.Length != 3)
            throw new ArgumentException("Coordinates cannot be non 3D-format");

        X = coordinates[0];
        Y = coordinates[1];
        Z = coordinates[2];

        Length = Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public Point Rotate(double alpha, double beta, double gamma)
    {
        double rotatedX = X * (Math.Cos(alpha) * Math.Cos(gamma) - Math.Cos(beta) * Math.Sin(alpha) * Math.Sin(gamma)) + 
                        Y * (-Math.Cos(gamma) * Math.Sin(alpha) - Math.Cos(alpha) * Math.Cos(beta) * Math.Sin(gamma)) + 
                        Z * Math.Sin(beta) * Math.Sin(gamma);

        double rotatedY = X * (Math.Cos(beta) * Math.Cos(gamma) * Math.Sin(alpha) + Math.Cos(alpha) * Math.Sin(gamma)) +
                        Y * (Math.Cos(alpha) * Math.Cos(beta) * Math.Cos(gamma) - Math.Sin(alpha) * Math.Sin(gamma)) +
                        Z * (-Math.Cos(gamma) * Math.Sin(beta));

        double rotatedZ = X * Math.Sin(alpha) * Math.Sin(beta) +
                        Y * Math.Cos(alpha) * Math.Sin(beta) +
                        Z * Math.Cos(beta);
        
        X = rotatedX;
        Y = rotatedY;
        Z = rotatedZ;

        return this;
    }

    public static Point operator -(Point p1, Point p2) { return new Point(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z); }
    public static Point operator *(Point p, double number) { return new Point(p.X * number, p.Y * number, p.Z * number); }

    public static double Distance(Point p1, Point p2) { return (p2 - p1).Length; }
    public static double Distance(Point p) { return p.Length; }
}