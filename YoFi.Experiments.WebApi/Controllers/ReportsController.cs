using Common.DotNet;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YoFi.Core.Reports;

namespace YoFi.Experiments.WebApi.Controllers
{
    /// <summary>
    /// Exposes an IReportEngine to the network
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IReportEngine _builder;
        private readonly IClock _clock;

        public ReportsController(ILogger<WeatherForecastController> logger, IReportEngine builder, IClock clock)
        {
            _logger = logger;
            _builder = builder;
            _clock = clock;
        }

        /// <summary>
        /// Returns the report definitions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<ReportDefinition>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var result = _builder.Definitions.ToList();
            return Ok(result);
        }

        /// <summary>
        /// Generates a report for a specific definition
        /// </summary>
        /// <param name="parameters"></param>
        [HttpPost]
        [ProducesResponseType(typeof(WireReport), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post(ReportParameters parameters)
        {
            try
            {
                var report = _builder.Build(parameters);
                var result = WireReport.BuildFrom(report);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Generates the summary reports
        /// </summary>
        /// <param name="parameters"></param>
        [HttpPost("Summary")]
        [ProducesResponseType(typeof(List<List<WireReport>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Summary(ReportParameters parameters)
        {
            var summary = _builder.BuildSummary(parameters);
            var result = summary
                .Select(g => g
                   .Select(r => WireReport.BuildFrom(r))
                   .ToList()
                )
                .ToList();
            return Ok(result);
        }
    }
}
