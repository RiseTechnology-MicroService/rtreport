using Microsoft.AspNetCore.Mvc;
using rtreport.Models;
using rtreport.Services;

namespace rtreport.Controllers
{
    [ApiController, Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService reportService;
        public ReportController(ReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet, Route("list")]
        public List<ReportSummaryModel> GetReportList()
        {
            return reportService.GetSummaryList();
        }

        [HttpGet, Route("detail/{requestId}")]
        public ReportDetailModel GetReportDetail(string requestId)
        {
            return reportService.GetReportDetail(requestId);
        }
    }
}
