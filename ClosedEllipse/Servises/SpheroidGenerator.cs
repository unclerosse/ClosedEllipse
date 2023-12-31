using ClosedEllipse.Models;

namespace ClosedEllipse.Services;

public class SpheroidGenerator
{
    private readonly int TryCount = 1_000;
    private readonly double Excess = 1.005;

    private List<Spheroid> Spheroids { get; set; } = new List<Spheroid>();
    private double GlobalVolume { get; set; } 
    private NumGenerator SemiAxisGenerator { get; set; }
    private NumGenerator CenterGenerator { get; set; }

    private readonly GenerationParamsDTO Request;
    private readonly ILogger _logger;

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

    public SpheroidGenerator(GenerationParamsDTO request, ILogger logger)
    {
        _logger = logger;
        _logger.LogInformation("Provided request: {request}", request.ToString());

        if (request.NumberOfItems <= 0 || (long)request.NumberOfItems != request.NumberOfItems)
            throw new ArgumentException("Number of items must be int and must be greater than or equal to one");

        if (request.NC == 0 && request.Rglobal == 0)
            throw new ArgumentException("NC and Rglobal cannot be zero at the same time");
        if (request.NC > 0 && request.Rglobal > 0)
            throw new ArgumentException("NC and Rglobal cannot be greater than zero at the same time");

        if (request.NC < 0)
            throw new ArgumentException("NC must be greater than or equal to zero");
        if (request.Rglobal < 0)
            throw new ArgumentException("Rglobal must be greater than or equal to zero");
        
        if (request.Eccentricity <= 0)
            throw new ArgumentException("Eccentricity must be greater than zero");
        
        if (request.SemiAxisDistribution == "uniform" && 
            (request.SemiAxes.Length != 2 || request.SemiAxes[0] <= 0 || request.SemiAxes[1] <= 0))
            throw new ArgumentException("Semi-axes must be greater than 0");  
        if (request.SemiAxisDistribution == "gauss" &&
            (request.SemiAxes.Length != 2 || request.SemiAxes[1] < 0))
            throw new ArgumentException("All params for Gauss distribution must be provided correctly");
        if (request.SemiAxisDistribution == "gamma" &&
            (request.SemiAxes.Length != 2 || request.SemiAxes[0] < 0 || request.SemiAxes[1] < 0))
            throw new ArgumentException("All params for Gamma distribution must be provided correctly");

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

        Request = request;
    }

    public List<Spheroid>? Generate()
    {
        var result = Generate((long)Request.NumberOfItems);

        if (result is null)
            _logger.LogWarning("Result is empty");
        else if (result.Count == (long)Request.NumberOfItems)
            _logger.LogInformation("Total items generated: {result}", result.Count);
        else
            _logger.LogWarning("Total items generated: {result} out of {NumberOfItems} planned", result.Count, (long)Request.NumberOfItems);
        
        return result;
    } 

    private List<Spheroid>? Generate(long numOfItems)
    {
        double localVolume = 0;
            
        for (var i = 0; i < numOfItems; ++i)
            {
                var item = new Spheroid(
                    Request.Eccentricity,
                    SemiAxisGenerator.Next(
                        Request.SemiAxes[0],
                        Request.SemiAxes[1]
                    ),
                    new Point(CenterGenerator.NextTriplet(
                        Request.Centers[0],
                        Request.Centers[1]
                    )),
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI),
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI),
                    new NumGenerator(new UniformDistribution()).Next(-Math.PI, Math.PI)
                    );

                localVolume += item.Volume;
                Spheroids.Add(item);
            }

        if (Request.Rglobal == 0)
        {
            GlobalVolume = localVolume / Request.NC;
            Request.Rglobal = Math.Cbrt(3 * GlobalVolume / (4 * Math.PI));
        }
        else if (Request.NC == 0)
        {
            GlobalVolume = 4.0 / 3.0 * Math.PI * Math.Pow(Request.Rglobal, 3);
            Request.NC = localVolume / GlobalVolume;
        }

        _logger.LogInformation(
            "Local volume: {localVolume} Global volume: {GlobalVolume} NC: {NC}", 
            localVolume, GlobalVolume, Request.NC
        );

        if (localVolume >= GlobalVolume || Request.NC > 0.4)
            return null;

        UpdateCoordinates(Spheroids);
        
        if (Spheroids.Count > 1)
            RemoveIntersections();                 

        return Spheroids;
    }

    private List<Spheroid>? RemoveIntersections() 
    {
        var deleted = new List<Spheroid>();
        for (int i = 0; i < Spheroids.Count - 1; ++i)
            for (int j = i + 1; j < Spheroids.Count; ++j)
                if (Spheroid.CheckIntersection(Spheroids[i], Spheroids[j]))
                {   
                    deleted.Add(Spheroids[j]);
                    Spheroids.RemoveAt(j);
                    --j;
                }

        UpdateCoordinates(deleted);
        
        for (int tryCount = TryCount; tryCount >= 0; --tryCount)
        {
            if (deleted.Count == 0)
                return null;

            if (tryCount == 0)
                break;

            foreach (var del in deleted.ToList())
            {
                bool hasIntersected = false;
                foreach (var spheroid in Spheroids)
                {
                    if (Spheroid.CheckIntersection(del, spheroid))
                    {   
                       hasIntersected = true;
                       break;
                    }
                }
                if (!hasIntersected)
                {
                    Spheroids.Add(del);
                    deleted.Remove(del);
                }
            }

            UpdateCoordinates(deleted);
        }

        _logger.LogWarning("Intersections found: {deleted}", deleted.Count);
        return deleted;
    }

    private void UpdateCoordinates(List<Spheroid> items)
    {
        foreach (var item in items.ToList())
        {  
            item.SetNewCoordinates(item.Coordinates * Request.Rglobal);
            for (var tryCount = TryCount; tryCount >= 0; --tryCount)
                if (!(Request.VolumeType == "cube" &&
                    item.Coordinates.X > -Request.Rglobal && item.Coordinates.X < Request.Rglobal &&
                    item.Coordinates.Y > -Request.Rglobal && item.Coordinates.Y < Request.Rglobal &&
                    item.Coordinates.Z > -Request.Rglobal && item.Coordinates.Z < Request.Rglobal) &&
                    !(Request.VolumeType == "sphere" &&
                    Point.Distance(item.Coordinates) < Request.Rglobal))
                    {
                        if (tryCount == 0)
                        {
                            _logger.LogWarning("Skipping object {item}", item);
                            items.Remove(item);
                        }
                        if (tryCount % 2 == 1)
                            Request.Rglobal /= Excess;
                            
                        item.SetNewCoordinates(new Point(CenterGenerator.NextTriplet(
                            Request.Centers[0],
                            Request.Centers[1]
                        )) * Request.Rglobal );

                        if (tryCount % 2 == 1)
                            Request.Rglobal *= Excess;
                        continue;
                    }
        }
    }
}