using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Entities
{
    public class EstimatedEmission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("site_id")]
        public int SiteId { get; set; }

        [Column("equipment_group_id")]
        public int EquipmentGroupId { get; set; }

        [Column("methane_in_kg")]
        public double MethaneInKg { get; set; }

        [Column("estimate_date")]
        public DateTime EstimateDate { get; set; }

        public required EmissionSite EmissionSite { get; set; }

        public required EquipmentGroup EquipmentGroup { get; set; }
    }
}
