using ClosedEllipse.Models;
using ClosedEllipse.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ClosedEllipse.Tests;

[TestFixture]
public class FunctionalTests
{
    [Test]
    public void TestSpheroidIntersection()
    {
        var request = new GenerationParamsDTO()
        {
            NumberOfItems = 503,
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
        ILogger<SpheroidGenerator> logger = LoggerFactory.Create(configure => {}).CreateLogger<SpheroidGenerator>();
        var gen = new SpheroidGenerator(request, logger);
        var numGen = new NumGenerator(new UniformDistribution());

        var result = gen.Generate() ?? throw new Exception("Objects cannot be generated");

        for (int i = 0; i < result.Count - 1; ++i)          
            for (int j = i + 1; j < result.Count; ++j)
                if (Point.Distance(result[i].Coordinates, result[j].Coordinates) <= result[i].SemiAxisA + result[j].SemiAxisA)
                    for (int k = 0; k < 100; ++k)
                    {
                        var point = result[i].PointRotation(new Point(
                            result[i].Coordinates.X + numGen.Next(-1, 1) * result[i].SemiAxisA,
                            result[i].Coordinates.Y + numGen.Next(-1, 1) * result[i].SemiAxisB, 
                            result[i].Coordinates.Z + numGen.Next(-1, 1) * result[i].SemiAxisB
                        ));

                        try
                        {
                            Assert.That(result[i].CheckPoint(point) && result[j].CheckPoint(point), Is.EqualTo(false));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex}");
                            Console.WriteLine($"{i}");
                            Console.WriteLine($"{result[i]}\n{result[j]}");
                        }
                    }
    }
}