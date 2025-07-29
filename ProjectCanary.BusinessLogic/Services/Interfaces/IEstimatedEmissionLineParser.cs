using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Interfaces
{
    public interface IEstimatedEmissionLineParser
    {
        EstimatedEmission ParseEmissions(string commaSeparatedEmissions);
    }
}
