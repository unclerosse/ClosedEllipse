namespace ClosedEllipse.Services;

public class NumGenerator
{
    private INumGenerationStrategy _strategy;

    public NumGenerator() 
    {
        _strategy = new UniformDistribution();
    }

    public NumGenerator(INumGenerationStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void SetStrategy(INumGenerationStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Next(double shape, double scale) 
    { 
        return _strategy.GenerateNumber(shape, scale); 
    }

    public double[] NextTriplet(double shape, double scale)
    {
        return new double[] 
        { 
            _strategy.GenerateNumber(shape, scale),
            _strategy.GenerateNumber(shape, scale),
            _strategy.GenerateNumber(shape, scale)
        };
    }
}

public interface INumGenerationStrategy 
{
    double GenerateNumber(double shape, double scale);
}