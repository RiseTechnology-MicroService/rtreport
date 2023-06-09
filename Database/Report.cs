namespace rtreport.Database
{
    public class Report
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CompletedDate { get; set; } = null;

        /// <summary>
        /// true = ready
        /// false = not ready
        /// </summary>
        public bool Status { get; set; } = false; 

        public ReportRequest ReportRequest { get; set; }
        public ReportResult? Result { get; set; } = null;

        public Report(string location, string requestId)
        {
            RequestDate = DateTime.Now;

            ReportRequest = new ReportRequest(location, requestId);
        }

        public void SetResult(ReportResult reportResult)
        {
            CompletedDate = DateTime.Now;
            Status = true;
            Result = reportResult;
        }
    }

    public class ReportRequest
    {
        public string RequestId { get; set; }
        public string Location { get; set; }
        public ReportRequest(string location, string requestId)
        {
            Location = location;
            RequestId = requestId;

        }
    }
    public class ReportResult
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }

        public ReportResult(string location, int personCount, int phoneNumberCount)
        {
            Location = location;
            PersonCount = personCount;
            PhoneNumberCount = phoneNumberCount;
        }
    }
}
