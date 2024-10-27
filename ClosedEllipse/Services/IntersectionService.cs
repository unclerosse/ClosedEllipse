using ClosedEllipse.Models;

namespace ClosedEllipse.Services;

public class IntersectionService
{
    public IResult CheckIntersection(SpheroidPairDto request)
    {
        var spheroid1 = new Spheroid(request.First);
        var spheroid2 = new Spheroid(request.Second);

        return Results.Ok(new 
        { 
            Result = Spheroid.CheckIntersection(spheroid1, spheroid2),
            FirstPoints = spheroid1.GetSlices(),
            SecondPoints = spheroid2.GetSlices()
        });
    }
}