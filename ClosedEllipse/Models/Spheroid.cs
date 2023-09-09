namespace ClosedEllipse.Models;

public record Spheroid
{
    public Point Coordinates { get; private set; }
    public double SemiMajorAxis { get; private set; }
    public double SemiMinorAxis { get; private set; }

    public double Eccentricity { get; private set; }

    public double EulerAngleX { get; private set; }
    public double EulerAngleY { get; private set; }
    public double EulerAngleZ { get; private set; }
    public double Volume { get; private set; }

    public Spheroid(double eccentricity, double semiAxis, Point center, double eulerAngleX, double eulerAngleY, double eulerAngleZ)
    {
        if (eccentricity < 0 || eccentricity > 1)
            throw new ArgumentException
                ("Eccentricity cannot be less than 0 or great than 1");
        Eccentricity = eccentricity;
        

        if (semiAxis <= 0)
            throw new ArgumentException
                ("The length of semi-axis cannnot be less than or equal to 0");
        
        SemiMajorAxis = semiAxis;
        SemiMinorAxis = Math.Sqrt((1 - Math.Pow(Eccentricity, 2)) * Math.Pow(SemiMajorAxis, 2));

        Coordinates = center;

        EulerAngleX = eulerAngleX;
        EulerAngleY = eulerAngleY;
        EulerAngleZ = eulerAngleZ;

        Volume = 4.0/3.0 * Math.PI * SemiMajorAxis * Math.Pow(SemiMinorAxis, 2);
    }

    public void SetNewCoordinates(Point point) { Coordinates = point; }
    
    public bool CheckPoint(Point point)
    {
        point = PointTransformation(point);

        var result = 
            (Math.Pow(point.X - Coordinates.X, 2) / Math.Pow(SemiMajorAxis, 2)) + 
            (Math.Pow(point.Y - Coordinates.Y, 2) / Math.Pow(SemiMinorAxis, 2)) + 
            (Math.Pow(point.Z - Coordinates.Z, 2) / Math.Pow(SemiMinorAxis, 2));

        return result <= 1;
    }

    protected List<Point> SliceSpheroid(int amount)
    {
        var result = new List<Point>();

        for (int i = 0; i < amount; i++)
        {
            double theta = 2 * Math.PI * i / amount; 
            double phi = Math.PI * i / amount;

            double x = SemiMinorAxis * Math.Cos(theta) * Math.Sin(phi);
            double y = SemiMajorAxis * Math.Sin(theta) * Math.Sin(phi);
            double z = SemiMinorAxis * Math.Cos(phi);

            result.Add(PointTransformation(new Point(Coordinates.X + x, Coordinates.Y + y, Coordinates.Z + z)));
        }
        
        return result;
    }

    public static bool CheckIntersection(Spheroid firstSpheroid, Spheroid secondSpheroid, int amount=500)
    {
        if (secondSpheroid.CheckPoint(firstSpheroid.Coordinates))
            return true;
        
        var points = firstSpheroid.SliceSpheroid(amount);
        for (var i = 0; i < points.Count; ++i)
            if (secondSpheroid.CheckPoint(points[i]))
                return true;

        return false;
    }
    
    public Point PointTransformation(Point point)
    {
        var x = point.X - Coordinates.X;
        var y = point.Y - Coordinates.Y;
        var z = point.Z - Coordinates.Z;

        double rotatedX = x * Math.Cos(EulerAngleY) * Math.Cos(EulerAngleZ) +
                        y * (Math.Cos(EulerAngleZ) * Math.Sin(EulerAngleX) * Math.Sin(EulerAngleY) - Math.Cos(EulerAngleX) * Math.Sin(EulerAngleZ)) +
                        z * (Math.Sin(EulerAngleX) * Math.Sin(EulerAngleZ) + Math.Cos(EulerAngleX) * Math.Cos(EulerAngleZ) * Math.Sin(EulerAngleY));

        double rotatedY = x * Math.Cos(EulerAngleY) * Math.Sin(EulerAngleZ) +
                        y * (Math.Cos(EulerAngleX) * Math.Cos(EulerAngleZ) + Math.Sin(EulerAngleX) * Math.Sin(EulerAngleY) * Math.Sin(EulerAngleZ)) +
                        z * (-Math.Cos(EulerAngleZ) * Math.Sin(EulerAngleX) + Math.Cos(EulerAngleX) * Math.Sin(EulerAngleY) * Math.Sin(EulerAngleZ));

        double rotatedZ = -x * Math.Sin(EulerAngleY) +
                        y * Math.Cos(EulerAngleY) * Math.Sin(EulerAngleX) +
                        z * Math.Cos(EulerAngleX) * Math.Cos(EulerAngleY);

        return new Point(rotatedX, rotatedY, rotatedZ);
    }
}