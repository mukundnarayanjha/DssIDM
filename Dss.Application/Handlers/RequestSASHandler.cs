using Confluent.Kafka;
using Dss.Application.Kafka.Messages.MRM;
using Dss.Application.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dss.Application.Handlers
{
    public class RequestSASHandler : IRequestHandler<RequestSASQuery, SASUrlRequest>
    {
        private readonly IOptions<ConsumerConfig> _configuration;

        public RequestSASHandler(IOptions<ConsumerConfig> configuration)
        {
            _configuration = configuration;
        }

        public async Task<SASUrlRequest> Handle(RequestSASQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // create the consumer
                var consumer = new ConsumerBuilder<Null, string>(_configuration.Value).Build();
                // subscribe to the specified topic
                //consumer.Subscribe(request.TopicName);
                consumer.Subscribe("SendNewFileToIDM");
                // while no data available wait
                while (true)
                {
                    // consume the data
                    var cr = consumer.Consume();
                    // close & commit
                    consumer.Close();
                    // return result
                    var result = JsonConvert.DeserializeObject<SASUrlRequest>(cr.Message.Value);
                    return await Task.FromResult(result);                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
