using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IMeasuredEmissionFileParser
    {
        IEnumerable<MeasuredEmission> ParseEmissions(string commaSeparatedEmissions);
    }
}
