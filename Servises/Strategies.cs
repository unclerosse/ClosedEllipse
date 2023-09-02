using MathNet.Numerics.Distributions;

namespace ClosedEllipse.Services;

public class GaussDistribution : IStrategy
{
    double IStrategy.GenerateNumber(double mean, double stddev)
    {
        return new Normal(mean, stddev).Sample();
    }
}

public class GammaDistribution : IStrategy
{
    public double GenerateNumber(double shape, double scale)
    {
        return new Gamma(shape, scale).Sample();
    }
}

public class UniformDistribution : IStrategy
{
    public double GenerateNumber(double mean, double stddev)
    {
        return new Random().NextDouble() * 2.0 - 1.0;
    }
}
