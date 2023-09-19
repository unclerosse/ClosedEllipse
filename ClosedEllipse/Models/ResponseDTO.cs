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
        X = double.Round(spheroid.Coordinates.X, 4);
        Y = double.Round(spheroid.Coordinates.Y, 4);
        Z = double.Round(spheroid.Coordinates.Z, 4);
        SemiMajorAxis = double.Round(spheroid.SemiMajorAxis, 8);
        SemiMinorAxis = double.Round(spheroid.SemiMinorAxis, 8);
        EulerAngleX = spheroid.EulerAngleX;
        EulerAngleY = spheroid.EulerAngleY;
        EulerAngleZ = spheroid.EulerAngleZ;
    }

}