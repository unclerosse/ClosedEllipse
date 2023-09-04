using ClosedEllipse.Models;

namespace ClosedEllipse.Services;

public class SpheroidGenerator
{
    private readonly int _TryCount = 5;
    private readonly double _Excess = 1.005;

    private List<Spheroid> _Spheroids { get; set; } = new List<Spheroid>();
    private NumGenerator _SemiAxisGenerator { get; set; }
    private NumGenerator _CenterGenerator { get; set; }

    private readonly RequestDTO _Request;

    private NumGenerator SetDistribution(NumGenerator generator, string distribution)
    {
        switch (distribution)
        {
        case "gauss":
            generator.SetStrategy(new GaussDistribution());
            break;
        case "gamma":
            generator.SetStrategy(new GammaDistribution());
            break;
        case "uniform":
            generator.SetStrategy(new UniformDistribution());
            break;
        default:
            throw new ArgumentException("Invalid distribution value");
        }

        return generator;
    }


    public SpheroidGenerator(RequestDTO request)
    {
        if (request.NumberOfItems <= 0 || (long)request.NumberOfItems != request.NumberOfItems)
            throw new ArgumentException("Number of items must be int and must be greater than or equal to one");
        
        if (request.NC <= 0)
            throw new ArgumentException("NC must be greater than zero");
        
        if (request.Eccentricity < 0 || request.Eccentricity >= 1)
            throw new ArgumentException("Eccentricity must be greater than or equal to zero and greater than one");
        
        if (request.SemiAxisDistribution == "uniform" && 
            (request.SemiAxes.Length != 2 || request.SemiAxes[0] <= 0 || request.SemiAxes[1] <= 0))
            throw new ArgumentException("Semi-axes must be greater than 0");  
        if (request.SemiAxisDistribution == "gauss" &&
            (request.SemiAxes.Length != 2 || request.SemiAxes[1] < 0))
            throw new ArgumentException("All params for Gauss distribution must be provided correctly");
        if (request.SemiAxisDistribution == "gamma" &&
            (request.SemiAxes.Length != 2 || request.SemiAxes[0] < 0 || request.SemiAxes[1] < 0))
            throw new ArgumentException("All params for Gamma distribution must be provided correctly");

        if (request.Rglobal <= 0)
            throw new ArgumentException("Global radius must be greater than zero");

        if (request.CenterDistribution == "gauss" &&
            (request.Centers.Length != 2 || request.Centers[1] < 0))
            throw new ArgumentException("All params for Gauss distribution must be provided correctly");
        if (request.CenterDistribution == "gamma" &&
            (request.Centers.Length != 2 || request.Centers[0] < 0 || request.Centers[1] < 0))
            throw new ArgumentException("All params for Gamma distribution must be provided correctly");
        if (request.NumberOfFiles <= 0)
            throw new ArgumentException("Number of files must be greater than or equal to one");

        _SemiAxisGenerator = SetDistribution(new NumGenerator(), request.SemiAxisDistribution);
        _CenterGenerator = SetDistribution(new NumGenerator(), request.CenterDistribution);

        _Request = request;
    }

    public List<Spheroid> GetSpheroids()
    {
        for (int i = 0; i < _Request.NumberOfItems; ++i)
        {
            _Spheroids.Add(
                new Spheroid(
                    _Request.Eccentricity, 
                    _SemiAxisGenerator.Next(
                        _Request.SemiAxes[0],
                        _Request.SemiAxes[1]
                    ),
                    new Point(_CenterGenerator.NextTriplet(
                        _Request.Centers[0],
                        _Request.Centers[1]
                    )) * _Request.Rglobal                 
                )
            );
        }
        
        return _Spheroids;
    }
}