using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.IpFiltering.Samples.StaticConfiguration.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("api/some-data")]
        public IActionResult GetSomeData()
        {
            return this.Ok("Some data");
        }

        [HttpGet("api/some-unguarded-data")]
        public IActionResult GetUnguardedData()
        {
            return this.Ok("Some unguarded data");
        }

        [HttpGet("api/some-data/{id}")]
        public IActionResult GetSpecificData(int id)
        {
            return this.Ok($"Specific data for id: {id}");
        }
    }
}