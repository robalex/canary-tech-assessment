using ProjectCanary.Data.Entities;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class EstimatedEmissionFileParser(
        IEquipmentGroupRetriever equipmentGroupRetriever, 
        ISiteCoordinateRetriever siteCoordinateRetriever,
        IEmissionSiteRetriever emissionSiteRetriever
        ) : IEstimatedEmissionFileParser
    {
        private const int LatitudeColumnIndex = 0;

        private const int LongitudeColumnIndex = 1;

        private const int EquipmentGroupNameColumnIndex = 2;

        private const int StartTimeColumnIndex = 3;

        private const int EndTimeColumnIndex = 4;

        private const int MethaneInKgColumnIndex = 5;

        private readonly IEquipmentGroupRetriever _equipmentGroupRetriever = equipmentGroupRetriever;

        private readonly ISiteCoordinateRetriever _siteCoordinateRetriever = siteCoordinateRetriever;

        private readonly IEmissionSiteRetriever _emissionSiteRetriever = emissionSiteRetriever;

        public EstimatedEmission ParseEmissions(string commaSeparatedEmissions)
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
            var methaneInKg = double.Parse(columns[MethaneInKgColumnIndex]);

            var equipmentGroup = equipmentGroupsByName[equipmentGroupName];
            EmissionSite? currentSite = EmissionSiteRetriever.GetClosestEmissionSiteForCoordinates(siteNameToCoordinates, siteNameToSite, latitude, longitude);
           
            return new EstimatedEmission()
            {
                EmissionSite = currentSite,
                SiteId = currentSite.SiteId,
                EstimateDate = startTime,
                MethaneInKg = methaneInKg,
                EquipmentGroup = equipmentGroup,
                EquipmentGroupId = equipmentGroup.EquipmentGroupId
            };
        }
    }
}
