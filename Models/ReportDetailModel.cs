namespace rtreport.Models
{
    public class ReportDetailModel
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }

        public ReportDetailModel(string location, int personCount, int phoneNumberCount)
        {
            Location = location;
            PersonCount = personCount;
            PhoneNumberCount = phoneNumberCount;
        }
    }
}
