using ProjectCanary.Data.Models;

namespace ProjectCanary.BusinessLogic.Services.Implementations
{
    public class EstimatedEmissionFileParser(
        IEquipmentGroupRetriever equipmentGroupRetriever, 
        ISiteCoordinateRetriever siteCoordinateRetriever,
        IEmissionSiteRetriever emissionSiteRetriever
        ) : IEstimatedEmissionFileParser
    {
        private IEquipmentGroupRetriever _equipmentGroupRetriever = equipmentGroupRetriever;

        private ISiteCoordinateRetriever _siteCoordinateRetriever = siteCoordinateRetriever;

        private IEmissionSiteRetriever _emissionSiteRetriever = emissionSiteRetriever;

        private const int LatitudeColumnIndex = 0;

        private const int LongitudeColumnIndex = 1;

        private const int EquipmentGroupNameColumnIndex = 2;

        private const int StartTimeColumnIndex = 3;

        private const int EndTimeColumnIndex = 4;

        private const int MethaneInKgColumnIndex = 5;

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
            EmissionSite? currentSite = EmissionSiteRetriever.GetEmissionSiteForCoordinates(siteNameToCoordinates, siteNameToSite, latitude, longitude);
           
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
