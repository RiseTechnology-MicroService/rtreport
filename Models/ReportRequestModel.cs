namespace rtreport.Models
{
    public class ReportRequestModel
    {
        public string RequestId { get; set; }
        public string Location { get; set; }

        public ReportRequestModel(string requestId, string location) 
        {
            RequestId = requestId;
            Location = location;
        }
    }
}
