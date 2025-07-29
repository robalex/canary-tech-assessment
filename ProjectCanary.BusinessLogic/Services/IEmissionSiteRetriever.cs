
using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEmissionSiteRetriever
    {
        Dictionary<string, EmissionSite> GetEmissionSitesByName();
    }
}
