using ProjectCanary.BusinessLogic.Models;

namespace ProjectCanary.BusinessLogic.Services;

public interface IEmissionsService
{
    /// <summary>
    /// Parses and ingests measured emissions data from a given input stream.
    /// </summary>
    /// <param name="inputStream">The input stream containing measured emissions data.</param>
    Task IngestMeasuredEmissionsAsync(Stream inputStream);

    /// <summary>
    /// Parses and ingests estimated emissions data from a given input stream.
    /// </summary>
    /// <param name="inputStream">The input stream containing estimated emissions data.</param>
    Task IngestEstimatedEmissionsAsync(Stream inputStream);

    /// <summary>
    /// Retrieves emissions data grouped by Site, Month, and Equipment Group for charting.
    /// </summary>
    /// <returns>A collection of structured data ready for chart representation.</returns>
    Task<IEnumerable<CombinedEmissionResults>> GetEmissionsChartDataAsync(EmissionComparisonGroupBy groupBy);
}