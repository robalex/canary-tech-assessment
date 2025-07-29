using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Interfaces
{
    public interface IEmissionSiteRetriever
    {
        Dictionary<string, EmissionSite> GetEmissionSitesByName();
    }
}
