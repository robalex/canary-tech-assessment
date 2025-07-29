namespace ProjectCanary.Api.Models
{
    public class MeasuredVsEstimatedResult
    {
        public required string Label { get; set; }

        public double MeasuredResult { get; set; }

        public double EstimatedResult { get; set; }
    }
}
