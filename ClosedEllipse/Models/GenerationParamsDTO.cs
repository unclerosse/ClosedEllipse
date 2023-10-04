namespace ClosedEllipse.Models;

public record GenerationParamsDTO
{
    public double NumberOfItems { get; set; }
    public double NC { get; set; }
    public double Eccentricity { get; set; } 
    public string SemiAxisDistribution { get; set; }
    public double[] SemiAxes { get; set; }
    public double Rglobal { get; set; }
    public string VolumeType { get; set; }
    public string CenterDistribution { get; set; }
    public double[] Centers { get; set; }
    public int NumberOfFiles { get; set; }
}