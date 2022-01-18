using Azure.Messaging.ServiceBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Mango.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _subscriptionEmail;
        private readonly string _orderUpdatePaymentResultTopic;
        private readonly EmailRepository _emailRepository;

        private ServiceBusProcessor orderUpdatePaymentStatusProcessor;

        private readonly IConfiguration _configuration;

        public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;
            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            _orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            orderUpdatePaymentStatusProcessor = client.CreateProcessor(_orderUpdatePaymentResultTopic, _subscriptionEmail);
        }

        public async Task Start()
        {
            orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await orderUpdatePaymentStatusProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            UpdatePaymentResultMessage objMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            try
            {
                await _emailRepository.SendAndLogEmail(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
