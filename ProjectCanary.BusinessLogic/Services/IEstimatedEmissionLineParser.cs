using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IEstimatedEmissionLineParser
    {
        EstimatedEmission ParseEmissions(string commaSeparatedEmissions);
    }
}
