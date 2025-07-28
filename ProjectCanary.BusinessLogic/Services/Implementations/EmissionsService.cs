using ProjectCanary.Data;
using ProjectCanary.Data.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectCanary.BusinessLogic.Models;
using System.Device.Location;

namespace ProjectCanary.BusinessLogic.Services.Implementations;

public class EmissionsService(ProjectCanaryDbContext db) : IEmissionsService
{
    private readonly ProjectCanaryDbContext _db = db;

    public async Task IngestMeasuredEmissionsAsync(Stream inputStream)
    {
        // TODO: parse the csv data from inputStream and save data to the database
        // NOTE: a single site can have many concurrent measured emissions from different equipment. 
        /**
         * It looks like latitude/longitude may be unique. Ask if they will be consistent or do we need to check to see if a measured/estimated lat/long is close to a site referende lat/long? I can check the data first to see if any differ.
         * Columns:
         * Latitude (double), Longitude (double), StartTime (DateTime), EndTime (DateTime), SiteName (string), EquipmentGroupName (string), EquipmentId (string), MethaneInKg (double)
         * Table1: Site (Id, Name, Latitude, Longitude)
         * Table2: EquipmentGroup (Id, SiteId, GroupName)
         * Table3: Equipment (Id, EquipmentGroupId, EquipmentName)
         * Table4: MeasuredEmissions (Id, EquipmentId, StartTime, EndTime, MethaneInKg)
         * Table5: EstimatedEmissions (Id, EquipmentId, StartTime, EndTime, MethaneInKg)
         * 
         * This is super normalized. We can figure out if we should denormalize later depending on performance.
         * 
         */
        var equipmentGroupsByName = GetEquipmentGroupsByName();
        var siteByCoordinates = GetEmissionSitesByCoordinates();
        var siteNameToCoordinates = GetEmissionSiteNameToCoordinates();
        var siteNameToSite = GetEmissionSitesByName();

        using var sr = new StreamReader(inputStream);
        string? firstLine = sr.ReadLine();
        if (firstLine == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        /*
        var columns = firstLine.Split(',');
        Dictionary<string, int> columnIndices = new();
        for (int i = 0; i < columns.Length; i++) {
            columnIndices.Add(columns[i].Trim(), i);
        }
        */

        string? line;
        var measuredEmissions = new List<MeasuredEmission>();

        int lineNumber = 1;
        int measurementsNotAtSite = 0;
        int measurementsAtSite = 0;

        try {
            while ((line = await sr.ReadLineAsync()) != null) {
                var columns = line.Split(',');
                var latitude = double.Parse(columns[0]);
                var longitude = double.Parse(columns[1]);
                var startTime = DateTime.SpecifyKind(DateTime.Parse(columns[2]), DateTimeKind.Utc);
                var endTime = DateTime.SpecifyKind(DateTime.Parse(columns[3]), DateTimeKind.Utc);
                var equipmentGroupName = columns[4];
                var equipmentId = Guid.Parse(columns[5]);
                var methaneInKg = double.Parse(columns[6]);

                var equipmentGroup = equipmentGroupsByName[equipmentGroupName];

                var measurementCoordinates = new GeoCoordinate(latitude, longitude);
                var minDistance = double.MaxValue;
                var currentSiteName = string.Empty;
                EmissionSite? currentSite = null;
                foreach (var siteName in siteNameToCoordinates.Keys) {
                    var distance = measurementCoordinates.GetDistanceTo(siteNameToCoordinates[siteName]);
                    if (distance < minDistance) {
                        minDistance = distance;
                        currentSite = siteNameToSite[siteName];
                    }
                }

                if (!siteByCoordinates.ContainsKey(latitude) || !siteByCoordinates[latitude].ContainsKey(longitude)) {
                    measurementsNotAtSite++;
                    //throw new ArgumentException("Input stream does not contain all required columns.");
                } else {
                    measurementsAtSite++;
                }

                //var site = siteByCoordinates[latitude][longitude];

                var measuredEmission = new MeasuredEmission()
                {
                    EmissionSite = currentSite,
                    SiteId = currentSite.SiteId,
                    MeasurementStartTime = startTime,
                    MeasurementEndTime = endTime,
                    EquipmentId = equipmentId,
                    MethaneInKg = methaneInKg,
                    EquipmentGroup = equipmentGroup,
                    EquipmentGroupId = equipmentGroup.EquipmentGroupId
                };

                measuredEmissions.Add(measuredEmission);

                if (measuredEmissions.Count >= 1000) {
                    // Save in batches to avoid memory issues
                    //await _db.MeasuredEmissions.AddRangeAsync(measuredEmissions);
                    //await _db.SaveChangesAsync();
                    await _db.BulkInsertOrUpdateAsync(measuredEmissions);
                    measuredEmissions.Clear();
                }

                lineNumber++;
            }
        }
        catch (Exception e) {
            var rob = 3;
        }

        if (measuredEmissions.Count > 0) {
            //await _db.MeasuredEmissions.AddRangeAsync(measuredEmissions);
            //await _db.SaveChangesAsync();
            await _db.BulkInsertOrUpdateAsync(measuredEmissions);
            measuredEmissions.Clear();
        }
    }

    private Dictionary<string, EquipmentGroup> GetEquipmentGroupsByName()
    {
        var equipmentGroups = _db.EquipmentGroups.ToList();
        Dictionary<string, EquipmentGroup> equipmentGroupByName = new Dictionary<string, EquipmentGroup>();
        foreach (var group in equipmentGroups) {
            if (equipmentGroupByName.ContainsKey(group.Name)) {
                throw new ArgumentException($"Duplicate equipment group found with name {group.Name}.");
            } else {
                equipmentGroupByName.Add(group.Name, group);
            }
        }
        return equipmentGroupByName;
    }

    private Dictionary<double, Dictionary<double, EmissionSite>> GetEmissionSitesByCoordinates()
    {
        var emissionSites = _db.EmissionSites.ToList();
        Dictionary<double, Dictionary<double, EmissionSite>> emissionSiteByLatLong = new Dictionary<double, Dictionary<double, EmissionSite>>();
        foreach (var site in emissionSites) {
            if (!emissionSiteByLatLong.ContainsKey(site.Latitude)) {
                emissionSiteByLatLong[site.Latitude] = new Dictionary<double, EmissionSite>();
            }

            if (emissionSiteByLatLong[site.Latitude].ContainsKey(site.Longitude)) {
                throw new ArgumentException($"Duplicate site found for latitude {site.Latitude} and longitude {site.Longitude}.");
            } else {
                emissionSiteByLatLong[site.Latitude].Add(site.Longitude, site);
            }
        }

        return emissionSiteByLatLong;
    }

    private Dictionary<string, EmissionSite> GetEmissionSitesByName()
    {
        var emissionSites = _db.EmissionSites.ToList();
        Dictionary<string, EmissionSite> emissionSiteByName = new Dictionary<string, EmissionSite>();
        foreach (var site in emissionSites) {
            if (emissionSiteByName.ContainsKey(site.Name)) {
                throw new ArgumentException($"Duplicate site found with name {site.Name}.");
            } else {
                emissionSiteByName.Add(site.Name, site);
            }
        }
        return emissionSiteByName;
    }

    private Dictionary<string, GeoCoordinate> GetEmissionSiteNameToCoordinates()
    {         
        var emissionSites = _db.EmissionSites.ToList();
        Dictionary<string, GeoCoordinate> emissionSiteNameToCoordinates = new Dictionary<string, GeoCoordinate>();
        foreach (var site in emissionSites) {
            if (emissionSiteNameToCoordinates.ContainsKey(site.Name)) {
                throw new ArgumentException($"Duplicate site found with name {site.Name}.");
            } else {
                emissionSiteNameToCoordinates.Add(site.Name, new GeoCoordinate(site.Latitude, site.Longitude));
            }
        }
        return emissionSiteNameToCoordinates;
    }

    public async Task IngestEstimatedEmissionsAsync(Stream inputStream)
    {
        // TODO: parse the csv data from inputStream and save data to the database

        var equipmentGroupsByName = GetEquipmentGroupsByName();
        var siteByCoordinates = GetEmissionSitesByCoordinates();
        var siteNameToCoordinates = GetEmissionSiteNameToCoordinates();
        var siteNameToSite = GetEmissionSitesByName();

        using var sr = new StreamReader(inputStream);
        string? firstLine = sr.ReadLine();
        if (firstLine == null) {
            throw new ArgumentException("Input stream is empty.");
        }

        /*
        var columns = firstLine.Split(',');
        Dictionary<string, int> columnIndices = new();
        for (int i = 0; i < columns.Length; i++) {
            columnIndices.Add(columns[i].Trim(), i);
        }
        */

        string? line;
        var estimatedEmissions = new List<EstimatedEmission>();

        while ((line = await sr.ReadLineAsync()) != null) {
            var columns = line.Split(',');
            var latitude = double.Parse(columns[0]);
            var longitude = double.Parse(columns[1]);
            var equipmentGroupName = columns[2];
            var startTime = DateTime.SpecifyKind(DateTime.Parse(columns[3]), DateTimeKind.Utc);
            var endTime = DateTime.SpecifyKind(DateTime.Parse(columns[4]), DateTimeKind.Utc);
            var methaneInKg = double.Parse(columns[5]);

            var equipmentGroup = equipmentGroupsByName[equipmentGroupName];

            var measurementCoordinates = new GeoCoordinate(latitude, longitude);
            var minDistance = double.MaxValue;
            var currentSiteName = string.Empty;
            EmissionSite? currentSite = null;
            foreach (var siteName in siteNameToCoordinates.Keys) {
                var distance = measurementCoordinates.GetDistanceTo(siteNameToCoordinates[siteName]);
                if (distance < minDistance) {
                    minDistance = distance;
                    currentSite = siteNameToSite[siteName];
                }
            }

            if (!siteByCoordinates.ContainsKey(latitude) || !siteByCoordinates[latitude].ContainsKey(longitude)) {
                //throw new ArgumentException("Input stream does not contain all required columns.");
            }

            //var site = siteByCoordinates[latitude][longitude];

            var estimatedEmission = new EstimatedEmission()
            {
                EmissionSite = currentSite,
                SiteId = currentSite.SiteId,
                EstimateStartTime = startTime,
                EstimateEndTime = endTime,
                MethaneInKg = methaneInKg,
                EquipmentGroup = equipmentGroup,
                EquipmentGroupId = equipmentGroup.EquipmentGroupId
            };

            estimatedEmissions.Add(estimatedEmission);

            if (estimatedEmissions.Count >= 1000) {
                // Save in batches to avoid memory issues
                //await _db.EstimatedEmissions.AddRangeAsync(estimatedEmissions);
                //await _db.SaveChangesAsync();
                await _db.BulkInsertOrUpdateAsync(estimatedEmissions);
                estimatedEmissions.Clear();
            }
        }

        if (estimatedEmissions.Count > 0) {
            //await _db.EstimatedEmissions.AddRangeAsync(estimatedEmissions);
            //await _db.SaveChangesAsync();
            await _db.BulkInsertOrUpdateAsync(estimatedEmissions);
        }
    }

    public async Task<IEnumerable<object>> GetEmissionsChartDataAsync(EmissionComparisonGroupBy? groupBy)
    {
        // TODO: query both measured and estimated emissions data from the database.
        //       - Reconcile the two data sources to provide meaningful comparisons.
        // TODO: return a collection of well-defined DTOs (update the return type from object) 
        //       that encapsulate measured and estimated totals.
        // TODO: ensure the returned data supports toggling between groupings by site, month, or equipment group.
        // NOTE: the final implementation should allow the end user to compare totals and visualize reconciliation/comparisons
        //       across the specified groupings.

        var measuredCollection = _db.MeasuredEmissions.AsQueryable();
        IQueryable<object> measuredResults = groupBy switch
        {
            EmissionComparisonGroupBy.EmissionSite => measuredCollection
                                .GroupBy(me => me.SiteId)
                                .Select(g => new
                                {
                                    SiteId = g.Key,
                                    TotalMethane = g.Sum(me => me.MethaneInKg)
                                }),
            EmissionComparisonGroupBy.EquipmentGroup => measuredCollection
                    .GroupBy(me => me.EquipmentGroupId)
                    .Select(g => new
                    {
                        EquipmentGroupId = g.Key,
                        TotalMethane = g.Sum(me => me.MethaneInKg)
                    }),
            EmissionComparisonGroupBy.YearAndMonth => measuredCollection
                    .GroupBy(me => new
                    {
                        me.MeasurementStartTime.Year,
                        me.MeasurementStartTime.Month
                    })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        TotalMethane = g.Sum(me => me.MethaneInKg)
                    }),
            null => throw new ArgumentException("Invalid grouping option", nameof(groupBy)),
            _ => throw new ArgumentException("Invalid grouping option", nameof(groupBy)),
        };
        var results = await measuredResults.ToListAsync();

        // The following variables (siteId, equipmentGroupId, year, month) are not defined in the provided code.
        // If you need to filter estimatedCollection, you should pass these as parameters to the method.

        var estimatedCollection = _db.EstimatedEmissions.AsQueryable();
        // Filtering logic for estimatedCollection should be added here if needed.

        var estimatedResults = await estimatedCollection.ToListAsync();

        // group by equipmentGroup, equipmentSite, month
        /**
         * From video
         * measured vs estimated emissions per month filter by equipment group and site?
         * charts for just measured or just estimated (monthly, daily, yearly, etc?)
         * 
         */

        /*
        _db.MeasuredEmissions.GroupBy(me => new { me.EquipmentGroupId, me.EmissionSiteId, Month = new DateTime(me.MeasurementStartTime.Year, me.MeasurementStartTime.Month, 1) })
            .Select(g => new
            {
                g.Key.EquipmentGroupId,
                g.Key.EmissionSiteId,
                g.Key.Month,
                TotalMeasuredMethaneInKg = g.Sum(me => me.MethaneInKg)
            });
        */

        /**
         * This is probably the most important section. We need to actually create a react UI that shows these totals and lets the user toggle.
         * It might be best to mock this up and work backwards.
         * 
         * 
         */
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<EquipmentGroup>> GetEquipmentGroupsAsync()
    {
        // Use ToListAsync to execute the query and return a list asynchronously
        var groups = await _db.EquipmentGroups.Distinct().ToListAsync();
        return groups.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<EmissionSite>> GetEmissionSitesAsync()
    {
        var sites = await _db.EmissionSites.Distinct().ToListAsync();
        return sites.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<YearAndMonth>> GetYearsAndMonthsAsync()
    {
        var estimatedYearsAndMonths = await _db.EstimatedEmissions
            .Select(e => new YearAndMonth
            {
                Year = e.EstimateStartTime.Year,
                Month = e.EstimateStartTime.Month
            }).Distinct()
            .ToListAsync();

        var measuredYearsAndMonths = await _db.MeasuredEmissions
            .Select(e => new YearAndMonth
            {
                Year = e.MeasurementStartTime.Year,
                Month = e.MeasurementStartTime.Month
            }).Distinct()
            .ToListAsync();

        var combinedYearsAndMonths = estimatedYearsAndMonths.Concat(measuredYearsAndMonths).Distinct();
        return combinedYearsAndMonths.OrderBy(y => y.Year).ThenBy(m => m.Month);
    }
}