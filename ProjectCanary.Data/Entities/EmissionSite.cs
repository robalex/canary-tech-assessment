using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Entities
{
    public class EmissionSite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SiteId { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
