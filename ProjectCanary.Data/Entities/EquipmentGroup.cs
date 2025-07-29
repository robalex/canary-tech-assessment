using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Entities
{
    public class EquipmentGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("equipment_group_id")]
        public int EquipmentGroupId { get; set; }

        [MaxLength(255)]
        [Column("name")]
        public required string Name { get; set; }
    }
}
