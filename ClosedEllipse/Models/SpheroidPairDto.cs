namespace ClosedEllipse.Models;

public record SpheroidRequestDto
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double SemiAxisA { get; set; }
    public double SemiAxisB { get; set; }
    public double EulerAngleX { get; set; }
    public double EulerAngleY { get; set; }
    public double EulerAngleZ { get; set; }

    public SpheroidRequestDto(double x, double y, double z, double semiAxisA, double semiAxisB, 
        double eulerAngleX, double eulerAngleY, double eulerAngleZ)
    {
        X = x;
        Y = y;
        Z = z;
        SemiAxisA = semiAxisA;
        SemiAxisB = semiAxisB;
        EulerAngleX = eulerAngleX;
        EulerAngleY = eulerAngleY;
        EulerAngleZ = eulerAngleZ;
    }
}

public record SpheroidPairDto
{
    public SpheroidRequestDto First { get; set; }
    public SpheroidRequestDto Second { get; set; }

    public SpheroidPairDto(SpheroidRequestDto first, SpheroidRequestDto second)
    {
        First = first;
        Second = second;
    }
}