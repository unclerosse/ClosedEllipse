using ClosedEllipse.Models;
using ClosedEllipse.Services;
using NUnit.Framework;

namespace ClosedEllipse.Tests;

[TestFixture]
public class FunctionalTests
{
    [Test]
    public void TestSpheroidIntersection()
    {
        var request = new RequestDTO()
        {
            NumberOfItems = 50003,
            NC = 1,
            Eccentricity = 0.51,
            SemiAxisDistribution = "uniform",
            SemiAxes = new double[2] { 250.0, 350.0 },
            Rglobal = 1e4 + 350,
            VolumeType = "sphere",
            CenterDistribution = "uniform",
            Centers = new double[2] { -1, 1,},
            NumberOfFiles = 1
        };

        var gen = new SpheroidGenerator(request);
        var numGen = new NumGenerator(new UniformDistribution());

        var result = gen.Generate() ?? throw new Exception("Objects cannot be generated");

        for (int i = 0; i < result.Count - 1; ++i)
        {
            Console.WriteLine($"Current object: {i}");
            
            for (int j = i + 1; j < result.Count; ++j)
                if (Point.Distance(result[i].Coordinates, result[j].Coordinates) <= result[i].SemiMajorAxis + result[j].SemiMajorAxis)
                    for (int k = 0; k < 300; ++k)
                    {
                        var point = result[i].PointTransformation(new Point(
                            numGen.Next(-1, 1) * result[i].SemiMajorAxis,
                            numGen.Next(-1, 1) * result[i].SemiMinorAxis, 
                            numGen.Next(-1, 1) * result[i].SemiMinorAxis
                        ));

                        Assert.That(result[i].CheckPoint(point) && result[j].CheckPoint(point), Is.EqualTo(false));
                    }
        }
    }
}