using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Models
{
    public class EquipmentGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipmentGroupId { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }
    }
}
