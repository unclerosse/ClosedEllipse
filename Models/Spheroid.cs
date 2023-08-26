namespace ClosedEllipse.Models;

public class Spheroid
{
    public Point Coordinates { get; private set; }
    public double SemiAxis { get; private set; }
    public double SemiMinorAxis { get; private set; }

    public double Eccentricity { get; private set; }

    public double Azimuth { get; private set; }
    public double Zenith { get; private set; }


    public Spheroid(double eccentricity, double semiAxis, double x, double y, double z, double azimuth, double zenith)
    {
        if (eccentricity < 0 || eccentricity > 1)
            throw new ArgumentOutOfRangeException
                (nameof(Eccentricity), "Eccentricity cannot be less than 0 or great than 1");
        Eccentricity = eccentricity;
        

        if (semiAxis <= 0)
            throw new ArgumentOutOfRangeException
                (nameof(SemiAxis), "The length of semi-axis cannnot be less than or equal to 0");
        
        SemiAxis = semiAxis;
        SemiMinorAxis = Math.Sqrt((1 - Math.Pow(Eccentricity, 2)) * Math.Pow(SemiAxis, 2));

        
        Coordinates = new(x, y, z);

        Azimuth = azimuth;
        Zenith = zenith;
    }

    protected bool CheckPoint(Point point)
    {
        var result = 
            (Math.Pow(point.X - Coordinates.X, 2) / Math.Pow(SemiAxis, 2)) + 
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

            double x = SemiAxis * Math.Cos(theta) * Math.Sin(phi);
            double y = SemiMinorAxis * Math.Sin(theta) * Math.Sin(phi);
            double z = SemiMinorAxis * Math.Cos(phi);

            result.Add(new Point(Coordinates.X + x, Coordinates.Y + y, Coordinates.Z + z));
        }
        
        return result;
    }

    public static bool CheckIntersection(Spheroid firstSpheroid, Spheroid secondSpheroid) 
    {
        var points = firstSpheroid.SliceSpheroid(500);
        for (var i = 0; i < points.Count; ++i)
            if (secondSpheroid.CheckPoint(points[i]))
                return true;

        return false;
    }
}