using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Models
{
    public class EstimatedEmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public int SiteId { get; set; }
        public int EquipmentGroupId { get; set; }

        public double MethaneInKg { get; set; }
        public DateTime EstimateStartTime { get; set; }
        public DateTime EstimateEndTime { get; set; }

        public required EmissionSite EmissionSite { get; set; }
        public required EquipmentGroup EquipmentGroup { get; set; }
    }
}
