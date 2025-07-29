using ProjectCanary.Data;
using ProjectCanary.Data.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectCanary.BusinessLogic.Models;
using ProjectCanary.BusinessLogic.Services.Interfaces;

namespace ProjectCanary.BusinessLogic.Services.Implementations;

public class EmissionsService(
    ProjectCanaryDbContext db,
    IEstimatedEmissionLineParser estimatedEmissionLineParser,
    IMeasuredEmissionLineParser measuredEmissionLineParser
    ) : IEmissionsService
{
    private readonly ProjectCanaryDbContext _db = db;

    private readonly IEstimatedEmissionLineParser _estimatedEmissionLineParser = estimatedEmissionLineParser;

    private readonly IMeasuredEmissionLineParser _measuredEmissionLineParser = measuredEmissionLineParser;

    public async Task IngestMeasuredEmissionsAsync(Stream inputStream)
    {
        using var sr = new StreamReader(inputStream);
        string? columnNameLine = sr.ReadLine();
        if (columnNameLine == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        string? line;
        var measuredEmissions = new List<MeasuredEmission>();

        while ((line = await sr.ReadLineAsync()) != null) {
            var currentMeasuredEmissions = _measuredEmissionLineParser.ParseEmissions(line);
            measuredEmissions.AddRange(currentMeasuredEmissions);

            if (measuredEmissions.Count >= 1000) {
                await _db.BulkInsertOrUpdateAsync(measuredEmissions);
                measuredEmissions.Clear();
            }
        }

        if (measuredEmissions.Count > 0) {
            await _db.BulkInsertOrUpdateAsync(measuredEmissions);
        }
    }

    public async Task IngestEstimatedEmissionsAsync(Stream inputStream)
    {
        using var sr = new StreamReader(inputStream);
        string? columnNameLine = sr.ReadLine();
        if (columnNameLine == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        string? line;
        var estimatedEmissions = new List<EstimatedEmission>();

        while ((line = await sr.ReadLineAsync()) != null) {
            var currentEstimatedEmissions = _estimatedEmissionLineParser.ParseEmissions(line);
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
            EmissionComparisonGroupBy.EmissionSite => await GroupByEmissionSite(),

            EmissionComparisonGroupBy.EquipmentGroup => await GroupByEquipmentGroup(),

            EmissionComparisonGroupBy.YearAndMonth => await GroupByYearAndMonth(),

            _ => throw new ArgumentException("Invalid grouping option", nameof(groupBy))
        };

        return results;
    }

    private async Task<List<CombinedEmissionResults>> GroupByYearAndMonth()
    {
        return await _db.MeasuredEmissions
                        .GroupBy(me => new
                        {
                            me.MeasurementDate.Year,
                            me.MeasurementDate.Month
                        })
                        .Select(g => new
                        {
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
                        .ToListAsync();
    }

    private async Task<List<CombinedEmissionResults>> GroupByEquipmentGroup()
    {
        return await _db.MeasuredEmissions
                        .GroupBy(me => new { me.EquipmentGroupId, me.EquipmentGroup.Name })
                        .Select(g => new CombinedEmissionResults
                        {
                            Label = g.Key.Name,
                            MeasuredResult = g.Sum(me => me.MethaneInKg),
                            EstimatedResult = _db.EstimatedEmissions
                                .Where(ee => ee.EquipmentGroupId == g.Key.EquipmentGroupId)
                                .Sum(ee => ee.MethaneInKg)
                        })
                        .ToListAsync();
    }

    private async Task<List<CombinedEmissionResults>> GroupByEmissionSite()
    {
        return await _db.MeasuredEmissions
                        .GroupBy(me => new { me.SiteId, me.EmissionSite.Name })
                        .Select(g => new CombinedEmissionResults
                        {
                            Label = g.Key.Name,
                            MeasuredResult = g.Sum(me => me.MethaneInKg),
                            EstimatedResult = _db.EstimatedEmissions
                                .Where(ee => ee.SiteId == g.Key.SiteId)
                                .Sum(ee => ee.MethaneInKg)
                        })
                        .ToListAsync();
    }
}