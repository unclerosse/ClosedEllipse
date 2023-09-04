using ClosedEllipse.Models;

namespace ClosedEllipse.Services;

public class SpheroidGenerator
{
    private readonly int TryCount = 5;
    // private readonly double Excess = 1.005;

    private List<Spheroid>? Spheroids { get; set; } = new List<Spheroid>();
    private double GlobalVolume { get; set; } 
    private NumGenerator SemiAxisGenerator { get; set; }
    private NumGenerator CenterGenerator { get; set; }

    private readonly RequestDTO Request;

    private static NumGenerator SetDistribution(NumGenerator generator, string distribution)
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

        SemiAxisGenerator = SetDistribution(new NumGenerator(), request.SemiAxisDistribution);
        CenterGenerator = SetDistribution(new NumGenerator(), request.CenterDistribution);

        if (request.VolumeType == "cube")
            GlobalVolume = Math.Pow(request.Rglobal * 2, 3);
        else if (request.VolumeType == "sphere")
            GlobalVolume = 4.0/3.0 * Math.PI * Math.Pow(request.Rglobal, 3);

        Request = request;
    }

    public List<Spheroid>? Generate()
    {
        var tryCount = TryCount;
        double localVolume = 0;
        for (int i = 0; i < Request.NumberOfItems; ++i)
        {
            if (tryCount == 0)
            {
                tryCount = TryCount;
                continue;
            }

            var item =  new Spheroid(
                    Request.Eccentricity, 
                    SemiAxisGenerator.Next(
                        Request.SemiAxes[0],
                        Request.SemiAxes[1]
                    ),
                    new Point(CenterGenerator.NextTriplet(
                        Request.Centers[0],
                        Request.Centers[1]
                    )) * Request.Rglobal,
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI),
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI),
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI)                                     
                );
            
            if (!(Request.VolumeType == "cube" &&
                item.Coordinates.X > -Request.Rglobal && item.Coordinates.X < Request.Rglobal &&
                item.Coordinates.Y > -Request.Rglobal && item.Coordinates.Y < Request.Rglobal &&
                item.Coordinates.Z > -Request.Rglobal && item.Coordinates.Z < Request.Rglobal) &&
                !(Request.VolumeType == "sphere" &&
                Point.Distance(item.Coordinates) < Request.Rglobal))
            {
                --tryCount;
                --i;
                continue;
            }

            localVolume += item.Volume; 
            
            Spheroids!.Add(item);
            tryCount = TryCount;
        }
        
        if (localVolume >= GlobalVolume)
            Spheroids = null;

        if (Spheroids != null && Spheroids.Count > 1)
            RemoveIntersections();

        return Spheroids;
    }

    private void RemoveIntersections() 
    {
        for (int i = 0; i < Spheroids!.Count - 1; ++i)
            for (int j = i + 1; j < Spheroids!.Count; ++j)
                if (Spheroid.CheckIntersection(Spheroids[i], Spheroids[j]))
                {
                    Console.WriteLine("Intersection is found");
                    Console.WriteLine($"Removed object: {Spheroids[j]}");
                    Spheroids.RemoveAt(j);
                    --j;
                }
    }


}