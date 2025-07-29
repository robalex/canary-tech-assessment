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
            var equipmentGroups = _db.EquipmentGroups.ToList();
            Dictionary<string, EquipmentGroup> equipmentGroupByName = new Dictionary<string, EquipmentGroup>();
            foreach (var group in equipmentGroups) {
                if (equipmentGroupByName.ContainsKey(group.Name)) {
                    throw new ArgumentException($"Duplicate equipment group found with name {group.Name}.");
                } else {
                    equipmentGroupByName.Add(group.Name, group);
                }
            }

            return equipmentGroupByName;
        }
    }
}
