namespace ProjectCanary.Api.Models
{
    public class EmissionReading
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string EquipmentGroupName { get; set; } = string.Empty;
        public double MethaneInKg { get; set; }
    }
}
