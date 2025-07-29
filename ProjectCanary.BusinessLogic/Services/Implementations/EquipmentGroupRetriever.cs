using ProjectCanary.BusinessLogic.Services.Interfaces;
using ProjectCanary.Data;
using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class EquipmentGroupRetriever(ProjectCanaryDbContext db) : IEquipmentGroupRetriever
    {
        private readonly ProjectCanaryDbContext _db = db;

        public Dictionary<string, EquipmentGroup> GetEquipmentGroupsByName()
        {
            return _db.EquipmentGroups
                .ToDictionary(
                    group => group.Name,
                    group => group,
                    StringComparer.Ordinal);
        }
    }
}
