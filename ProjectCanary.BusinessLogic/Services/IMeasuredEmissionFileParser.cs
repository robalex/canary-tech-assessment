using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services
{
    public interface IMeasuredEmissionFileParser
    {
        IEnumerable<MeasuredEmission> ParseEmissions(string commaSeparatedEmissions);
    }
}
