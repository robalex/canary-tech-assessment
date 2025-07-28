namespace ProjectCanary.Api.Models
{
    public class MeasuredVsEstimatedResult
    {
        /**
         * The UI expectes a list of labels
         * Then is expects a list of data per result set
         * 
         * ie. labels = ["January", "February", "March"]
         *      estimatedResults = [100, 200, 300]
         *      measuredResults = [110, 190, 310]
         *      
         *      going to try this
         *      {
         *         "results": [
         *           { "month": "January", "estimated": 100, "measured": 110 },
         *           { "month": "February", "estimated": 200, "measured": 190 }
         *        ]
         *      }
         */
        public required string Label { get; set; }

        public float MeasuredResult { get; set; }

        public float EstimatedResult { get; set; }
    }
}
