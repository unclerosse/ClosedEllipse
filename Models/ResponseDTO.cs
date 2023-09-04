namespace ClosedEllipse.Models;

public record ResponseDTO
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double SemiMajorAxis { get; set; }
    public double SemiMinorAxis { get; set; }
    public double EulerAngleX { get; set; }
    public double EulerAngleY { get; set; }
    public double EulerAngleZ { get; set; }

    public ResponseDTO(Spheroid spheroid)
    {
        X = spheroid.Coordinates.X;
        Y = spheroid.Coordinates.Y;
        Z = spheroid.Coordinates.Z;
        SemiMajorAxis = spheroid.SemiMajorAxis;
        SemiMinorAxis = spheroid.SemiMinorAxis;
        EulerAngleX = spheroid.EulerAngleX;
        EulerAngleY = spheroid.EulerAngleY;
        EulerAngleZ = spheroid.EulerAngleZ;
    }

}