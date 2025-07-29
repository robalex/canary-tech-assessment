using GeoCoordinatePortable;
using ProjectCanary.BusinessLogic.Services.Interfaces;
using ProjectCanary.Data;
using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class EmissionSiteRetriever(ProjectCanaryDbContext db) : IEmissionSiteRetriever
    {
        private readonly ProjectCanaryDbContext _db = db;

        public Dictionary<string, EmissionSite> GetEmissionSitesByName()
        {
            return _db.EmissionSites
                .ToDictionary(
                    site => site.Name,
                    site => site,
                    StringComparer.Ordinal);
        }

        public static EmissionSite GetClosestEmissionSiteForCoordinates(
            Dictionary<string, GeoCoordinate> siteNameToCoordinates, 
            Dictionary<string, EmissionSite> siteNameToSite, 
            double latitude, 
            double longitude
            )
        {
            var measurementCoordinates = new GeoCoordinate(latitude, longitude);
            var minDistance = double.MaxValue;

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
