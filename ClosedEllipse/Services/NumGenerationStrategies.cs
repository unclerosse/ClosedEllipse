using MathNet.Numerics.Distributions;

namespace ClosedEllipse.Services;

public class GaussDistribution : INumGenerationStrategy
{
    public double GenerateNumber(double mean, double stddev)
    {
        return new Normal(mean, stddev).Sample();
    }
}

public class GammaDistribution : INumGenerationStrategy
{
    public double GenerateNumber(double shape, double scale)
    {
        return new Gamma(shape, scale).Sample() * 2 - 1;
    }
}

public class UniformDistribution : INumGenerationStrategy
{
    public double GenerateNumber(double lower, double upper)
    {
        return lower + new Random().NextDouble() * (upper - lower);
    }
}
