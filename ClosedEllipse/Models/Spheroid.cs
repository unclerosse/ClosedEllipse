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

    public Spheroid(SpheroidRequestDto request)
    {
        Coordinates = new Point(request.X, request.Y, request.Z);
        SemiAxisA = request.SemiAxisA;
        SemiAxisB = request.SemiAxisB;

        Eccentricity = request.SemiAxisB / request.SemiAxisA;

        EulerAngleX = request.EulerAngleX;
        EulerAngleY = request.EulerAngleY;
        EulerAngleZ = request.EulerAngleZ;

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

        return double.Round(result, 8) <= 1;
    }

    protected List<Point> SliceSpheroid(int amount)
    {
        var result = new List<Point>();
        
        int numZPoints = (int)Math.Sqrt(amount);
        int numXYPoints = amount / numZPoints;

        for (int i = 0; i < numZPoints; i++)
        {
            double phi = Math.PI * i / (numZPoints - 1);
            double z = SemiAxisB * Math.Cos(phi);

            double radiusX = SemiAxisA * Math.Sin(phi);
            double radiusY = SemiAxisB * Math.Sin(phi);

            for (int j = 0; j < numXYPoints; j++)
            {
                double theta = 2 * Math.PI * j / numXYPoints;

                double x = radiusX * Math.Cos(theta);
                double y = radiusY * Math.Sin(theta);

                result.Add(PointRotation(new (Coordinates.X + x, Coordinates.Y + y, Coordinates.Z + z)));
            }
        }

        return result;
    }


    public static bool CheckIntersection(Spheroid firstSpheroid, Spheroid secondSpheroid)
    {
        var semiAxesSum = Math.Max(firstSpheroid.SemiAxisA, firstSpheroid.SemiAxisB) +
            Math.Max(secondSpheroid.SemiAxisA, secondSpheroid.SemiAxisB);

        var distance = Point.Distance(firstSpheroid.Coordinates, secondSpheroid.Coordinates);
        
        if (distance > semiAxesSum)
            return false;

        if (firstSpheroid.Eccentricity == 1 && secondSpheroid.Eccentricity == 1)
            return true;
        
        if (secondSpheroid.CheckPoint(firstSpheroid.Coordinates))
            return true;
        
        var points = firstSpheroid.GetSlices();
        return points.Any(secondSpheroid.CheckPoint);
    }
    
    public Point PointTransformation(Point point)
    {
        return point - Coordinates;
    }

    public Point PointRotation(Point point)
    {
        point -= Coordinates;
        point = point.Rotate(EulerAngleX, EulerAngleY, EulerAngleZ);
        point += Coordinates;
        return point;
    }

    public Point NegativePointRotation(Point point)
    {
        point -= Coordinates;
        point = point.Rotate(-EulerAngleX, -EulerAngleY, -EulerAngleZ);
        point += Coordinates;
        return point;
    }

    public Point[] GetSlices() 
    {
        return [.. SliceSpheroid(500)];
    }
}