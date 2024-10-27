using ClosedEllipse.Models;

namespace ClosedEllipse.Servises;

public class SpheroidSliceService
{
    public List<Point> GetSlices(SpheroidRequestDto request)
    {
        var spheroid = new Spheroid(request);
        return [.. spheroid.GetSlices()];
    }
}