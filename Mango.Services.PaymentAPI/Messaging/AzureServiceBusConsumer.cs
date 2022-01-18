using Azure.Messaging.ServiceBus;
using Mango.Services.PaymentAPI.Messages;
using MangoMessageBus;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer :IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _subscriptionPayment;
        private readonly string _orderPaymentProcessTopic;
        private readonly string _orderupdatepaymentresulttopic;

        private ServiceBusProcessor orderPaymentProcessor;

        private readonly IProcessorPayment _processPayment;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;

        public AzureServiceBusConsumer(IConfiguration configuration, IMessageBus messageBus, IProcessorPayment processPayment)
        {
            _processPayment = processPayment;
            _configuration = configuration;
            _messageBus = messageBus;

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
            _orderupdatepaymentresulttopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");
            _orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopics");

            var client = new ServiceBusClient(_serviceBusConnectionString);

            orderPaymentProcessor = client.CreateProcessor(_orderPaymentProcessTopic, _subscriptionPayment);
        }

        public async Task Start()
        {
            orderPaymentProcessor.ProcessMessageAsync += ProcessPayment;
            orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
            await orderPaymentProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await orderPaymentProcessor.StopProcessingAsync();
            await orderPaymentProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task ProcessPayment(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId,
                Email = paymentRequestMessage.Email
            };

            try
            {
                await _messageBus.PublishMessage(updatePaymentResultMessage, _orderupdatepaymentresulttopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
