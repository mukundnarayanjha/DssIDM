using Dss.Application.Kafka.Messages.UserRegistration;
using Dss.Application.Kafka.Messages.MRM;
using Kafka.Constants;
using Kafka.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dss.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaOperationController : ControllerBase
    {
        private readonly IKafkaProducer<string, RegisterUser> _kafkaProducer;
        private readonly IKafkaProducer<string, SASUrlRequest> _kafkaMRMProducer;
        public KafkaOperationController(
            IKafkaProducer<string, RegisterUser> kafkaProducer,
            IKafkaProducer<string, SASUrlRequest> kafkaMRMProducer
            )
        {
            _kafkaProducer = kafkaProducer;
            _kafkaMRMProducer = kafkaMRMProducer;
        }

        [HttpPost]
        [Route("GenerateSASUrl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateSASUrl(SASUrlRequest sasUrlRequest)
        {
            await _kafkaProducer.ProduceAsync(KafkaTopics.SendNewFileToIDM, null, sasUrlRequest);

            return Ok("SAS Url generation is in progress");
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
