using rtreport.Models;
using rtreport.Responses;
using System.Text.Json;

namespace rtreport.Services
{
    public class ReportSupplierService
    {
        private readonly HttpClient _httpClient;
        public ReportSupplierService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration.GetValue<string>("Services:rtpersonURL")!);
        }
        public async Task<ReportResultModel> GetReport(string location)
        {
            var response = await _httpClient.GetAsync($"api/persons/get-report/{location}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    PropertyNameCaseInsensitive = true,
                };
                var reportResult = JsonSerializer.Deserialize<GenericResponse<ReportResultModel>>(content, options);
                return reportResult!.Data!;
            }

            throw new Exception("Report supplier service is not available");
        }
    }
}
