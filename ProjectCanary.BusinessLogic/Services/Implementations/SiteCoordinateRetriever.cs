using GeoCoordinatePortable;
using ProjectCanary.Data;
using ProjectCanary.Data.Models;

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

        public Dictionary<double, Dictionary<double, EmissionSite>> GetEmissionSitesByCoordinates()
        {
            var emissionSites = _db.EmissionSites.ToList();
            Dictionary<double, Dictionary<double, EmissionSite>> emissionSiteByLatLong = new Dictionary<double, Dictionary<double, EmissionSite>>();
            foreach (var site in emissionSites) {
                if (!emissionSiteByLatLong.ContainsKey(site.Latitude)) {
                    emissionSiteByLatLong[site.Latitude] = new Dictionary<double, EmissionSite>();
                }

                if (emissionSiteByLatLong[site.Latitude].ContainsKey(site.Longitude)) {
                    throw new ArgumentException($"Duplicate site found for latitude {site.Latitude} and longitude {site.Longitude}.");
                } else {
                    emissionSiteByLatLong[site.Latitude].Add(site.Longitude, site);
                }
            }

            return emissionSiteByLatLong;
        }
    }
}
