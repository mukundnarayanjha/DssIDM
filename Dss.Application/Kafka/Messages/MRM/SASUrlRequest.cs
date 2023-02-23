namespace Dss.Application.Kafka.Messages.MRM
{
    public class SASUrlRequest
    { 
        public string UserName { get; set; }
        public string TopicName { get; set; }
        public bool SasUrl { get; set; }
    }
}
