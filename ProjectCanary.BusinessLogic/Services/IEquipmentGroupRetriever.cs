using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEquipmentGroupRetriever
    {
        Dictionary<string, EquipmentGroup> GetEquipmentGroupsByName();
    }
}
