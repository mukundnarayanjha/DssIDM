using Dss.Application.Kafka.Messages.UserRegistration;
using Kafka.Constants;
using Kafka.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace dss.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaOperationController : ControllerBase
    {
        private readonly IKafkaProducer<string, RegisterUser> _kafkaProducer;
        public KafkaOperationController(IKafkaProducer<string, RegisterUser> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ProduceMessage(RegisterUser request)
        {
            await _kafkaProducer.ProduceAsync(KafkaTopics.RegisterUser, null, request);

            return Ok("User Registration In Progress");
        }
    }
}
