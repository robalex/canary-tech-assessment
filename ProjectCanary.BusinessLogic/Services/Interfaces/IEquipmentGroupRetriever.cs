using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Interfaces
{
    public interface IEquipmentGroupRetriever
    {
        Dictionary<string, EquipmentGroup> GetEquipmentGroupsByName();
    }
}
