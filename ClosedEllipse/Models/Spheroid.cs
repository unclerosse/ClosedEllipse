namespace ClosedEllipse.Models;

public record Spheroid
{
    public Point Coordinates { get; private set; }
    public double SemiAxisA { get; private set; }
    public double SemiAxisB { get; private set; }

    public double Eccentricity { get; private set; }

    public double EulerAngleX { get; private set; }
    public double EulerAngleY { get; private set; }
    public double EulerAngleZ { get; private set; }
    public double Volume { get; private set; }

    public Spheroid(double eccentricity, double semiAxis, Point center, double eulerAngleX, double eulerAngleY, double eulerAngleZ)
    {
        if (eccentricity <= 0)
            throw new ArgumentException
                ("Eccentricity cannot be equal to 0");
        Eccentricity = eccentricity;
        
        if (semiAxis <= 0)
            throw new ArgumentException
                ("The length of semi-axis cannnot be less than or equal to 0");
        
        SemiAxisA = semiAxis;
        SemiAxisB = semiAxis * eccentricity;

        Coordinates = center;

        EulerAngleX = eulerAngleX;
        EulerAngleY = eulerAngleY;
        EulerAngleZ = eulerAngleZ;

        Volume = 4.0/3.0 * Math.PI * SemiAxisA * Math.Pow(SemiAxisB, 2);
    }

    public void SetNewCoordinates(Point point) { Coordinates = point; }
    
    public bool CheckPoint(Point point)
    {
        point = NegativePointRotation(PointTransformation(point));

        var result = 
            (Math.Pow(point.X, 2) / Math.Pow(SemiAxisA, 2)) + 
            (Math.Pow(point.Y, 2) / Math.Pow(SemiAxisB, 2)) + 
            (Math.Pow(point.Z, 2) / Math.Pow(SemiAxisB, 2));

        return double.Round(result, 14) <= 1;
    }

    protected List<Point> SliceSpheroid(int amount)
    {
        var result = new List<Point>()
        {
            PointRotation(new(Coordinates.X - SemiAxisA, Coordinates.Y, Coordinates.Z)),
            PointRotation(new(Coordinates.X + SemiAxisA, Coordinates.Y, Coordinates.Z)),
            PointRotation(new(Coordinates.X, Coordinates.Y - SemiAxisB, Coordinates.Z)),
            PointRotation(new(Coordinates.X, Coordinates.Y + SemiAxisB, Coordinates.Z)),
            PointRotation(new(Coordinates.X, Coordinates.Y, Coordinates.Z - SemiAxisB)),
            PointRotation(new(Coordinates.X, Coordinates.Y, Coordinates.Z + SemiAxisB)),
        };
        
        // for (int i = 0; i < amount; i++)
        // {
        //     double phi = 2 * Math.PI * i / amount; 
        //     double theta = Math.PI * i / amount;

        //     double x = SemiAxisA * Math.Cos(phi) * Math.Sin(theta);
        //     double y = SemiAxisB * Math.Sin(phi) * Math.Sin(theta);
        //     double z = SemiAxisB * Math.Cos(theta);

        //     result.Add(PointRotation(new Point(Coordinates.X + x, Coordinates.Y + y, Coordinates.Z + z)));
        // }
        
        return result;
    }

    public static bool CheckIntersection(Spheroid firstSpheroid, Spheroid secondSpheroid, int amount=500)
    {
        if (Point.Distance(firstSpheroid.Coordinates, secondSpheroid.Coordinates) >
            firstSpheroid.SemiAxisA + secondSpheroid.SemiAxisA)
            return false;
        
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

        return new Point(x, y, z);
    }

    public Point PointRotation(Point point)
    {
        return point.Rotate(EulerAngleX, EulerAngleY, EulerAngleZ);
    }

    public Point NegativePointRotation(Point point)
    {
        return point.Rotate(-EulerAngleX, -EulerAngleY, -EulerAngleZ);
    }

    public Point[] GetSlices() 
    {
        return SliceSpheroid(500).ToArray();
    }
}