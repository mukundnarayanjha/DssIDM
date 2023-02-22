using Dss.API.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dss.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        private readonly RequestQueryHandler _handler;
        public ConsumerController(RequestQueryHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        ///    Consume data from a topic
        /// </summary>
        /// <param name="request">holds the name of target topic </param>
        /// <returns> consumed message from topic</returns>
        [HttpPost]
        public async Task<IActionResult> Consume(RequestQuery request)
        {
            try
            {
                var result = await _handler.Handle(request);
                return await Task.FromResult(Ok(result));
            }
            catch (Exception e)
            {
                return await Task.FromResult(StatusCode(500, e.ToString()));
            }
        }
    }
}

