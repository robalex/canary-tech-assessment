using Microsoft.AspNetCore.Mvc;
using ProjectCanary.Api.Models;
using ProjectCanary.BusinessLogic.Models;
using ProjectCanary.BusinessLogic.Services.Interfaces;
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
            try {
                using var inputStream = file.OpenReadStream();
                await _emissionsService.IngestMeasuredEmissionsAsync(inputStream);
            } catch(Exception e) {
                return StatusCode(500, "Failed to ingest measured emissions");
            }

            return Ok();
        }

        [HttpPost("estimated")]
        public async Task<IActionResult> UploadEstimatedEmissions(IFormFile file)
        {
            try {
                using var inputStream = file.OpenReadStream();
                await _emissionsService.IngestEstimatedEmissionsAsync(inputStream);
            } catch {
                return StatusCode(500, "Failed to ingest estimated emissions");
            }

            return Ok();
        }

        [HttpGet("measured-vs-estimated")]
        public async Task<IActionResult> GetMeasuredVsEstimatedChartData(EmissionComparisonGroupBy groupBy)
        {
            try {
                var chartData = await _emissionsService.GetEmissionsChartDataAsync(groupBy);
                var result = chartData.Select(data => new MeasuredVsEstimatedResult
                {
                    Label = groupBy == EmissionComparisonGroupBy.YearAndMonth ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(data.Label)) : data.Label,
                    MeasuredResult = data.MeasuredResult,
                    EstimatedResult = data.EstimatedResult
                }).ToList();

                return Ok(result);
            } catch {
                return StatusCode(500, "Failed to retrieve emissions data");
            }
        }
    }
}
