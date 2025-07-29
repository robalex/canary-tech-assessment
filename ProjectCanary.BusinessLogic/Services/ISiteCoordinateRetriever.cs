
using GeoCoordinatePortable;
using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface ISiteCoordinateRetriever
    {
        Dictionary<string, GeoCoordinate> GetEmissionSiteNameToCoordinates();
        
        Dictionary<double, Dictionary<double, EmissionSite>> GetEmissionSitesByCoordinates();
    }
}
