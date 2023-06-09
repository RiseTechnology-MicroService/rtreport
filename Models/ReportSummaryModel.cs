namespace rtreport.Models
{
    public class ReportSummaryModel
    {
        public string RequestId { get; set; }
        public bool Status { get; set; }
        public string StatusText
        {
            get
            {
                if (Status)
                    return "Ready";

                else
                    return "Not Ready";
            }
        }

        public ReportSummaryModel(string requestId, bool status)
        {
            RequestId = requestId;
            Status = status;
        }
    }
}
