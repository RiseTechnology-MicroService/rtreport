using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using rtreport.Models;
using System.Text;
using System.Text.Json;

namespace rtreport.Services
{
    public class ReportRequestService
    {
        private readonly ReportService reportService;
        private static readonly object lockObject = new object();
        private string hostname;
        private int port;
        private string queueName;
        public ReportRequestService(ReportService reportService, IConfiguration configuration)
        {
            this.reportService = reportService;
            hostname = configuration.GetValue<string>("MessageBrokerSettings:Hostname")!;
            port = configuration.GetValue<int>("MessageBrokerSettings:Port");
            queueName = configuration.GetValue<string>("MessageBrokerSettings:QueueName")!;

            ListenTheQueue();
        }

        public void ListenTheQueue()
        {
            if (!Monitor.TryEnter(lockObject))
                return;

            try
            {
                var factory = new ConnectionFactory() { HostName = hostname, Port = port };
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                {
                    channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            var requestModel = JsonSerializer.Deserialize<ReportRequestModel>(message);

                            if (requestModel != null)
                            {
                                await reportService.AddRequest(requestModel);
                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    };
                    channel.BasicConsume(queue: queueName,
                                         autoAck: false,
                                         consumer: consumer);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Monitor.Exit(lockObject);
        }
    }
}
