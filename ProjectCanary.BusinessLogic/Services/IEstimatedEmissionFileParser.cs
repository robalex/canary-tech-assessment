using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEstimatedEmissionFileParser
    {
        EstimatedEmission ParseEmissions(string commaSeparatedEmissions);
    }
}
