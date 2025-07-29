using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IMeasuredEmissionLineParser
    {
        IEnumerable<MeasuredEmission> ParseEmissions(string commaSeparatedEmissions);
    }
}
