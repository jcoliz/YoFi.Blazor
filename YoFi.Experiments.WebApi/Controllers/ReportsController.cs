using Common.DotNet;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YoFi.Core.Reports;

namespace YoFi.Experiments.WebApi.Controllers
{
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
        /// <returns></returns>
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
        /// <param name="definition"></param>
        [HttpPost]
        [ProducesResponseType(typeof(Report), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Post([FromBody] ReportDefinition definition)
        {
            return Ok();
        }

        /// <summary>
        /// Generates the summary reports
        /// </summary>
        /// <param name="definition"></param>
        [HttpPost("Summary")]
        [ProducesResponseType(typeof(List<List<Report>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Summary([FromBody] ReportDefinition definition)
        {
            return Ok();
        }
    }
}
