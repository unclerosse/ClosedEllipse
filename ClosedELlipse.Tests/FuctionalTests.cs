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
            NumberOfItems = 5003,
            NC = 0.37,
            Eccentricity = 0.51,
            SemiAxisDistribution = "uniform",
            SemiAxes = new double[2] { 250.0, 350.0 },
            Rglobal = 0,
            VolumeType = "sphere",
            CenterDistribution = "uniform",
            Centers = new double[2] { -1, 1 },
            NumberOfFiles = 1
        };

        var gen = new SpheroidGenerator(request);
        var numGen = new NumGenerator(new UniformDistribution());

        var result = gen.Generate() ?? throw new Exception("Objects cannot be generated");

        for (int i = 0; i < result.Count - 1; ++i)          
            for (int j = i + 1; j < result.Count; ++j)
                if (Point.Distance(result[i].Coordinates, result[j].Coordinates) <= result[i].SemiMajorAxis + result[j].SemiMajorAxis)
                    for (int k = 0; k < 100; ++k)
                    {
                        var point = result[i].PointRotation(new Point(
                            result[i].Coordinates.X + numGen.Next(-1, 1) * result[i].SemiMajorAxis,
                            result[i].Coordinates.Y + numGen.Next(-1, 1) * result[i].SemiMinorAxis, 
                            result[i].Coordinates.Z + numGen.Next(-1, 1) * result[i].SemiMinorAxis
                        ));

                        try
                        {
                            Assert.That(result[i].CheckPoint(point) && result[j].CheckPoint(point), Is.EqualTo(false));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.StackTrace}");
                            Console.WriteLine($"{result[i]}\n{result[j]}");
                        }
                    }
    }
}