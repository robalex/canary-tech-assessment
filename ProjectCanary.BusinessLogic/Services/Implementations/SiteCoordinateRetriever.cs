using GeoCoordinatePortable;
using ProjectCanary.Data;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class SiteCoordinateRetriever(ProjectCanaryDbContext db) : ISiteCoordinateRetriever
    {
        private readonly ProjectCanaryDbContext _db = db;

        public Dictionary<string, GeoCoordinate> GetEmissionSiteNameToCoordinates()
        {
            var emissionSites = _db.EmissionSites.ToList();
            Dictionary<string, GeoCoordinate> emissionSiteNameToCoordinates = new Dictionary<string, GeoCoordinate>();
            foreach (var site in emissionSites) {
                if (emissionSiteNameToCoordinates.ContainsKey(site.Name)) {
                    throw new ArgumentException($"Duplicate site found with name {site.Name}.");
                } else {
                    emissionSiteNameToCoordinates.Add(site.Name, new GeoCoordinate(site.Latitude, site.Longitude));
                }
            }
            return emissionSiteNameToCoordinates;
        }
    }
}
