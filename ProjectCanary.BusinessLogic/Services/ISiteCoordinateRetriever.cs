using GeoCoordinatePortable;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface ISiteCoordinateRetriever
    {
        Dictionary<string, GeoCoordinate> GetEmissionSiteNameToCoordinates();
    }
}
