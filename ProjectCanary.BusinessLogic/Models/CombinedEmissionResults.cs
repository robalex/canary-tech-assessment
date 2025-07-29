namespace ProjectCanary.BusinessLogic.Models
{
    public class CombinedEmissionResults
    {
        public required string Label { get; set; }

        public double MeasuredResult { get; set; }

        public double EstimatedResult { get; set; }
    }
}
