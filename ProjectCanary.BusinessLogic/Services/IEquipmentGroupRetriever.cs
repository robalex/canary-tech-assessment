
using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEquipmentGroupRetriever
    {
        Dictionary<string, EquipmentGroup> GetEquipmentGroupsByName();
    }
}
