using GeoCoordinatePortable;

namespace ProjectCanary.BusinessLogic.Services.Interfaces
{
    public interface ISiteCoordinateRetriever
    {
        Dictionary<string, GeoCoordinate> GetEmissionSiteNameToCoordinates();
    }
}
