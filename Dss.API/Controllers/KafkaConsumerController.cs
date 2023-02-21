using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace dss.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaConsumerController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public IActionResult Get()
        {
            return Ok("Welcome to Kafka Consumer Application.");
        }
    }
}
