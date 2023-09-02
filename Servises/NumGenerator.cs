namespace ClosedEllipse.Services;

public class NumGenerator
{
    private IStrategy _strategy;

    public NumGenerator(IStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Next(double mean, double stddev) 
    { 
        return _strategy.GenerateNumber(mean, stddev); 
    }

    public double[] NextTriplet(double mean, double stddev)
    {
        return new double[] 
        { 
            _strategy.GenerateNumber(mean, stddev),
            _strategy.GenerateNumber(mean, stddev),
            _strategy.GenerateNumber(mean, stddev) 
        };
    }
}

public interface IStrategy 
{
    double GenerateNumber(double mean, double stddev);
}