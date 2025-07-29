using ProjectCanary.Data;
using ProjectCanary.Data.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectCanary.BusinessLogic.Models;

namespace ProjectCanary.BusinessLogic.Services.Implementations;

public class EmissionsService(
    ProjectCanaryDbContext db,
    IEstimatedEmissionFileParser estimatedEmissionFileParser,
    IMeasuredEmissionFileParser measuredEmissionFileParser
    ) : IEmissionsService
{
    private readonly ProjectCanaryDbContext _db = db;

    private readonly IEstimatedEmissionFileParser _estimatedEmissionFileParser = estimatedEmissionFileParser;

    private readonly IMeasuredEmissionFileParser _measuredEmissionFileParser = measuredEmissionFileParser;

    public async Task IngestMeasuredEmissionsAsync(Stream inputStream)
    {
        using var sr = new StreamReader(inputStream);
        string? line = sr.ReadLine();
        if (line == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        var measuredEmissions = new List<MeasuredEmission>();

        try {
            while ((line = await sr.ReadLineAsync()) != null) {
                var currentMeasuredEmissions = _measuredEmissionFileParser.ParseEmissions(line);
                measuredEmissions.AddRange(currentMeasuredEmissions);

                if (measuredEmissions.Count >= 1000) {
                    await _db.BulkInsertOrUpdateAsync(measuredEmissions);
                    measuredEmissions.Clear();
                }
            }
        }
        catch (Exception e) {
            var rob = 3;
        }

        if (measuredEmissions.Count > 0) {
            await _db.BulkInsertOrUpdateAsync(measuredEmissions);
        }
    }

    public async Task IngestEstimatedEmissionsAsync(Stream inputStream)
    {
        using var sr = new StreamReader(inputStream);
        string? line = sr.ReadLine();
        if (line == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        var estimatedEmissions = new List<EstimatedEmission>();

        while ((line = await sr.ReadLineAsync()) != null) {
            var currentEstimatedEmissions = _estimatedEmissionFileParser.ParseEmissions(line);
            estimatedEmissions.Add(currentEstimatedEmissions);

            if (estimatedEmissions.Count >= 1000) {
                await _db.BulkInsertOrUpdateAsync(estimatedEmissions);
                estimatedEmissions.Clear();
            }
        }

        if (estimatedEmissions.Count > 0) {
            await _db.BulkInsertOrUpdateAsync(estimatedEmissions);
        }
    }

    public async Task<IEnumerable<CombinedEmissionResults>> GetEmissionsChartDataAsync(EmissionComparisonGroupBy groupBy)
    {
        var results = groupBy switch
        {
            EmissionComparisonGroupBy.EmissionSite => await _db.MeasuredEmissions
                .GroupBy(me => new { me.SiteId, me.EmissionSite.Name })
                .Select(g => new CombinedEmissionResults
                {
                    Label = g.Key.Name,
                    MeasuredResult = g.Sum(me => me.MethaneInKg),
                    EstimatedResult = _db.EstimatedEmissions
                        .Where(ee => ee.SiteId == g.Key.SiteId)
                        .Sum(ee => ee.MethaneInKg)
                })
                .ToListAsync(),

            EmissionComparisonGroupBy.EquipmentGroup => await _db.MeasuredEmissions
                .GroupBy(me => new { me.EquipmentGroupId, me.EquipmentGroup.Name })
                .Select(g => new CombinedEmissionResults
                {
                    Label = g.Key.Name,
                    MeasuredResult = g.Sum(me => me.MethaneInKg),
                    EstimatedResult = _db.EstimatedEmissions
                        .Where(ee => ee.EquipmentGroupId == g.Key.EquipmentGroupId)
                        .Sum(ee => ee.MethaneInKg)
                })
                .ToListAsync(),

            EmissionComparisonGroupBy.YearAndMonth => await _db.MeasuredEmissions
                .GroupBy(me => new {
                    me.MeasurementDate.Year,
                    me.MeasurementDate.Month
                })
                .Select(g => new {
                    g.Key.Month,
                    Label = g.Key.Month.ToString(),
                    MeasuredResult = g.Sum(me => me.MethaneInKg),
                    EstimatedResult = _db.EstimatedEmissions
                        .Where(ee => ee.EstimateDate.Year == g.Key.Year &&
                                     ee.EstimateDate.Month == g.Key.Month)
                        .Sum(ee => ee.MethaneInKg)
                })
                .OrderBy(r => r.Month)
                .Select(r => new CombinedEmissionResults
                {
                    Label = r.Label,
                    MeasuredResult = r.MeasuredResult,
                    EstimatedResult = r.EstimatedResult
                })
                .ToListAsync(),

            _ => throw new ArgumentException("Invalid grouping option", nameof(groupBy))
        };

        return results;
    }
}