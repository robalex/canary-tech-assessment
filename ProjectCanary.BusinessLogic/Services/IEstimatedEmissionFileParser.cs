
using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEstimatedEmissionFileParser
    {
        EstimatedEmission ParseEmissions(string commaSeparatedEmissions);
    }
}
