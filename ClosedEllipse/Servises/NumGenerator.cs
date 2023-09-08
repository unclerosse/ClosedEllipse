namespace ClosedEllipse.Services;

public class NumGenerator
{
    private IStrategy _strategy;

    public NumGenerator() { }

    public NumGenerator(IStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void SetStrategy(IStrategy strategy)
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

public interface IStrategy 
{
    double GenerateNumber(double shape, double scale);
}