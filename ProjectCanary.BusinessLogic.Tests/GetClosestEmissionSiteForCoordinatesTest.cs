using ProjectCanary.BusinessLogic.Services.Implementations;
using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Tests
{
    public class Tests
    {
        private List<EmissionSite> _testEmissionSites = new List<EmissionSite>()
        {
            new EmissionSite {
                SiteId = 1,
                Name = "Site A",
                Latitude = 34.0522,
                Longitude = -118.2437
            },
            new EmissionSite {
                SiteId = 2,
                Name = "Site B",
                Latitude = 36.1699,
                Longitude = -115.1398
            },
            new EmissionSite {
                SiteId = 3,
                Name = "Site C",
                Latitude = 40.7128,
                Longitude = -74.0060
            }
        };

        private Dictionary<string, EmissionSite> _testEmissionSiteByName;

        private Dictionary<string, GeoCoordinatePortable.GeoCoordinate> _siteNameToCoordinates;

        [SetUp]
        public void Setup()
        {
            _testEmissionSiteByName = _testEmissionSites.ToDictionary(site => site.Name, site => site);
            _siteNameToCoordinates = _testEmissionSites.ToDictionary(site => site.Name, site => new GeoCoordinatePortable.GeoCoordinate(site.Latitude, site.Longitude));
        }

        [Test]
        public void ClosestSiteShouldBeFound()
        {
             var closestSite = EmissionSiteRetriever.GetClosestEmissionSiteForCoordinates(_siteNameToCoordinates, _testEmissionSiteByName, 34.500, -118.00);

            Assert.That(closestSite.Name, Is.EqualTo("Site A"));
        }

        [Test]
        public void EmptySiteNameToCoordinatesShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => EmissionSiteRetriever.GetClosestEmissionSiteForCoordinates(
                new Dictionary<string, GeoCoordinatePortable.GeoCoordinate>(),
                _testEmissionSiteByName,
                0.0,
                0.0
            ));
        }

        [Test]
        public void EmptySiteNameToSiteShouldThrowException()
        {
            Assert.Throws<KeyNotFoundException>(() => EmissionSiteRetriever.GetClosestEmissionSiteForCoordinates(
                _siteNameToCoordinates,
                new Dictionary<string, EmissionSite>(),
                0.0,
                0.0
            ));
        }
    }
}