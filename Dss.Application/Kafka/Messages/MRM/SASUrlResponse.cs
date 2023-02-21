namespace Dss.Application.Kafka.Messages.MRM
{
    public class SASUrlResponse
    {  
        public string UserName { get; set; }     
        public Uri SasUrl { get; set; }
    }
}
