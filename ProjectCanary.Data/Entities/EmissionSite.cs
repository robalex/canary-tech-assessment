using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectCanary.Data.Entities
{
    public class EmissionSite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("site_id")]
        public int SiteId { get; set; }

        [MaxLength(255)]
        [Column("name")]
        public required string Name { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }
    }
}
