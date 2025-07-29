using Microsoft.AspNetCore.Mvc;
using ProjectCanary.Api.Models;
using ProjectCanary.BusinessLogic.Models;
using ProjectCanary.BusinessLogic.Services;
using System.Globalization;

namespace ProjectCanary.Api.Controllers
{
    [ApiController]
    [Route("emissions")]
    public class EmissionsController(IEmissionsService emissionsService) : ControllerBase
    {
        private readonly IEmissionsService _emissionsService = emissionsService;

        [HttpPost("measured")]
        public async Task<IActionResult> UploadMeasuredEmissions(IFormFile file)
        {
            using var inputStream = file.OpenReadStream();

            await _emissionsService.IngestMeasuredEmissionsAsync(inputStream);
            //TODO: return errors if ingestion fails

            return Ok();
        }

        [HttpPost("estimated")]
        public async Task<IActionResult> UploadEstimatedEmissions(IFormFile file)
        {
            using var inputStream = file.OpenReadStream();

            await _emissionsService.IngestEstimatedEmissionsAsync(inputStream);
            //TODO: return errors if ingestion fails

            return Ok();
        }

        [HttpGet("measured-vs-estimated")]
        public async Task<IActionResult> GetMeasuredVsEstimatedChartData(EmissionComparisonGroupBy groupBy)
        {
            var chartData = await _emissionsService.GetEmissionsChartDataAsync(groupBy);
            var result = chartData.Select(data => new MeasuredVsEstimatedResult
            {
                Label = groupBy == EmissionComparisonGroupBy.YearAndMonth ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(data.Label)) : data.Label,
                MeasuredResult = data.MeasuredResult,
                EstimatedResult = data.EstimatedResult
            }).ToList();

            return Ok(result);
        }

        [HttpGet("get-equipment-groups")]
        public async Task<IActionResult> GetEquipmentGroups()
        {
            var equipmentGroups = await _emissionsService.GetEquipmentGroupsAsync();
            return Ok(equipmentGroups);
        }

        [HttpGet("get-emission-sites")]
        public async Task<IActionResult> GetEmissionSites()
        {
            var emissionSites = await _emissionsService.GetEmissionSitesAsync();
            return Ok(emissionSites);
        }

        [HttpGet("get-years-and-months")]
        public async Task<IActionResult> GetYearsAndMonths()
        {
            var yearsAndMonths = await _emissionsService.GetYearsAndMonthsAsync();
            return Ok(yearsAndMonths);
        }
    }
}
