//-----------------------------------------------------------------------------------
// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
//-----------------------------------------------------------------------------------

using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProcessImageEvents.Models;

namespace ProcessImageEvents.AzFunctions
{
    public class ProcessImageFunction
    {
        private readonly ILogger _logger;

        public ProcessImageFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProcessImageFunction>();
        }

        [Function(nameof(ProcessImageFunction))]
        //[EventGridOutput(TopicEndpointUri = "MyEventGridTopicUriSetting", TopicKeySetting = "MyEventGridTopicKeySetting")]
        public async Task Run([EventGridTrigger] ImageEvent input, FunctionContext context)
        {
            var logger = context.GetLogger(nameof(ProcessImageFunction));

            // ------------------Capture Event and create logs-----------------------------
            logger.LogInformation("Event Recieved...................!");
            logger.LogInformation("Event ID:" + input.Id);
            logger.LogInformation("Event Data: " + input.Data.ToString());

            // ------------------Convert Event Data Json to object and get image name-----------------------------

            var eventData = JsonConvert.DeserializeObject<EventData>(input.Data.ToString());

            var imageUploaded = eventData?.url.Substring(eventData.url.LastIndexOf('/') + 1);

            logger.LogInformation("Uploaded Image Name: " + imageUploaded);


            // ------------------Send messages to Azure Service Bus-----------------------------
            var serviceBusConnectionString = "Endpoint=sb://vv00imgservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManagedSharedAccessKeys;SharedAccessKey=ImDqRVwvvGJL8cU1QQVaHVjeBTO5zicE8+ASbHe0UR4=;EntityPath=vv00imgqueue";

            var queueName = "vv00imgqueue";

            // since ServiceBusClient implements IAsyncDisposable we create it with "await using"
            await using var client = new ServiceBusClient(serviceBusConnectionString);

            // create the sender
            ServiceBusSender sender = client.CreateSender(queueName);

            // create a message that we can send. UTF-8 encoding is used when providing a string.
            var messageValue = $"{DateTime.Now}:New Image Uploaded - Image Name:  {imageUploaded} - Image Type: {eventData.contentType}";

            logger.LogInformation("Publishing message to service bus: " + messageValue);

            ServiceBusMessage message = new ServiceBusMessage(messageValue);

            // send the message
            await sender.SendMessageAsync(message);

            logger.LogInformation("Message Published..........!");

            logger.LogInformation("---------------------Testing GitHub Actions Deploy----------------------------------");
            logger.LogInformation("---------------------Test - 1 ----------------------------------");
            logger.LogInformation("---------------------Test - 2 ----------------------------------");

        }
    }
}
