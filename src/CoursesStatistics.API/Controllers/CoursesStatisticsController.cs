using CoursesStatistics.API.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesStatistics.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CoursesStatisticsController : ControllerBase
    {

        [HttpGet("CoursesAges")]
        [Route("CoursesAges")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> CoursesAges([FromServices] ICoursesStatisticsQueries coursesStatisticsQueries)
        {
            var coursesAges = await coursesStatisticsQueries.CoursesAges();

            return Ok(coursesAges);
        }
    }

}
