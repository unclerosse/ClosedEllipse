namespace ClosedEllipse.Models;

public record ResponseDTO
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double SemiAxis { get; set; }
    public double SemiMinorAxis { get; set; }
    // public double EulerAngleX { get; set; }
    // public double EulerAngleY { get; set; }
    // public double EulerAngleZ { get; set; }

    public ResponseDTO(Spheroid spheroid)
    {
        X = spheroid.Coordinates.X;
        Y = spheroid.Coordinates.Y;
        Z = spheroid.Coordinates.Z;
        SemiAxis = spheroid.SemiAxis;
        SemiMinorAxis = spheroid.SemiMinorAxis;
    }

}