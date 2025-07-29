using GeoCoordinatePortable;
using ProjectCanary.Data;
using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class EmissionSiteRetriever(ProjectCanaryDbContext db) : IEmissionSiteRetriever
    {
        private readonly ProjectCanaryDbContext _db = db;

        public Dictionary<string, EmissionSite> GetEmissionSitesByName()
        {
            var emissionSites = _db.EmissionSites.ToList();
            Dictionary<string, EmissionSite> emissionSiteByName = new Dictionary<string, EmissionSite>();
            foreach (var site in emissionSites) {
                if (emissionSiteByName.ContainsKey(site.Name)) {
                    throw new ArgumentException($"Duplicate site found with name {site.Name}.");
                } else {
                    emissionSiteByName.Add(site.Name, site);
                }
            }

            return emissionSiteByName;
        }

        public static EmissionSite GetClosestEmissionSiteForCoordinates(Dictionary<string, GeoCoordinate> siteNameToCoordinates, Dictionary<string, EmissionSite> siteNameToSite, double latitude, double longitude)
        {
            var measurementCoordinates = new GeoCoordinate(latitude, longitude);
            var minDistance = double.MaxValue;
            var currentSiteName = string.Empty;

            EmissionSite? currentSite = null;
            foreach (var siteName in siteNameToCoordinates.Keys) {
                var distanceInMeters = measurementCoordinates.GetDistanceTo(siteNameToCoordinates[siteName]);
                if (distanceInMeters < minDistance) {
                    minDistance = distanceInMeters;
                    currentSite = siteNameToSite[siteName];
                }
            }

            if (currentSite == null) {
                throw new ArgumentException($"No emission site found for coordinates ({latitude}, {longitude}).");
            }

            return currentSite;
        }
    }
}
