using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using rtreport.Database;
using rtreport.Models;
using System.Text;
using System.Text.Json;

namespace rtreport.Services
{
    public class ReportService
    {
        private readonly IMongoCollection<Report> collection;

        public ReportService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            collection = mongoDatabase.GetCollection<Report>(databaseSettings.Value.ReportCollectionName);
        }

        public List<ReportSummaryModel> GetSummaryList()
        {
            return collection.Find(_ => true).Project(s => new ReportSummaryModel(s.ReportRequest.RequestId, s.Status)).ToList();
        }

        public ReportDetailModel GetReportDetail(string requestId)
        {
            return collection.Find(s => s.Status && s.ReportRequest.RequestId == requestId).Project(s => new ReportDetailModel(s.Result!.Location, s.Result.PersonCount, s.Result.PhoneNumberCount)).FirstOrDefault();
        }

        public List<Report> GetList(int count, bool status = false)
        {
            try
            {
                return collection.Find(s => s.Status == status).Limit(count).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddRequest(ReportRequestModel request)
        {
            try
            {
                await collection.InsertOneAsync(new Report(request.Location, request.RequestId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateRequest(Report report)
        {
            try
            {
                collection.ReplaceOne(r => r.Id == report.Id, report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateRequestAsync(Report report)
        {
            try
            {
                await collection.ReplaceOneAsync(r => r.Id == report.Id, report);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
