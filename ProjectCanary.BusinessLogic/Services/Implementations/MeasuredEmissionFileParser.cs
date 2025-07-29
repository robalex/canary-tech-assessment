using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class MeasuredEmissionFileParser(
        IEquipmentGroupRetriever equipmentGroupRetriever,
        ISiteCoordinateRetriever siteCoordinateRetriever,
        IEmissionSiteRetriever emissionSiteRetriever
        ) : IMeasuredEmissionFileParser
    {
        private const int LatitudeColumnIndex = 0;

        private const int LongitudeColumnIndex = 1;

        private const int StartTimeColumnIndex = 2;

        private const int EndTimeColumnIndex = 3;

        private const int EquipmentGroupNameColumnIndex = 4;

        private const int EquipmentIdColumnIndex = 5;

        private const int MethaneInKgColumnIndex = 6;

        private readonly IEquipmentGroupRetriever _equipmentGroupRetriever = equipmentGroupRetriever;

        private readonly ISiteCoordinateRetriever _siteCoordinateRetriever = siteCoordinateRetriever;

        private readonly IEmissionSiteRetriever _emissionSiteRetriever = emissionSiteRetriever;

        public IEnumerable<MeasuredEmission> ParseEmissions(string commaSeparatedEmissions)
        {
            var equipmentGroupsByName = _equipmentGroupRetriever.GetEquipmentGroupsByName();
            var siteNameToCoordinates = _siteCoordinateRetriever.GetEmissionSiteNameToCoordinates();
            var siteNameToSite = _emissionSiteRetriever.GetEmissionSitesByName();

            var columns = commaSeparatedEmissions.Split(',');
            var latitude = double.Parse(columns[LatitudeColumnIndex]);
            var longitude = double.Parse(columns[LongitudeColumnIndex]);
            var startTime = DateTime.SpecifyKind(DateTime.Parse(columns[StartTimeColumnIndex]), DateTimeKind.Utc);
            var endTime = DateTime.SpecifyKind(DateTime.Parse(columns[EndTimeColumnIndex]), DateTimeKind.Utc);
            var equipmentGroupName = columns[EquipmentGroupNameColumnIndex];
            var equipmentId = Guid.Parse(columns[EquipmentIdColumnIndex]);
            var methaneInKg = double.Parse(columns[MethaneInKgColumnIndex]);

            var equipmentGroup = equipmentGroupsByName[equipmentGroupName];
            EmissionSite? currentSite = EmissionSiteRetriever.GetClosestEmissionSiteForCoordinates(siteNameToCoordinates, siteNameToSite, latitude, longitude);

            var totalDays = (endTime - startTime).TotalDays;
            var methanePerDay = methaneInKg / totalDays;

            var measuredEmissions = new List<MeasuredEmission>();
            for (var date = startTime; date < endTime; date = date.AddDays(1)) {
                var emission = new MeasuredEmission()
                {
                    EmissionSite = currentSite,
                    SiteId = currentSite.SiteId,
                    MeasurementDate = date,
                    EquipmentId = equipmentId,
                    MethaneInKg = methanePerDay,
                    EquipmentGroup = equipmentGroup,
                    EquipmentGroupId = equipmentGroup.EquipmentGroupId
                };

                measuredEmissions.Add(
                    emission
                );
            }

            return measuredEmissions;
        }
    }
}
