using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Interfaces
{
    public interface IMeasuredEmissionLineParser
    {
        IEnumerable<MeasuredEmission> ParseEmissions(string commaSeparatedEmissions);
    }
}
