using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEmissionSiteRetriever
    {
        Dictionary<string, EmissionSite> GetEmissionSitesByName();
    }
}
