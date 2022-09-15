using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

public class GetEventDataFunction
{
    [Function("KafkaTrigger")]
    public static void Run(
        [KafkaTrigger("BrokerList",
                        "topic",
                        Username = "$ConnectionString",
                        Password = "EventHubConnectionString",
                        Protocol = BrokerProtocol.SaslSsl,
                        AuthenticationMode = BrokerAuthenticationMode.Plain,
                        ConsumerGroup = "$Default")] string eventData, FunctionContext context)
    {
        var logger = context.GetLogger("KafkaFunction");
        logger.LogInformation($"C# Kafka trigger function processed a message: {JObject.Parse(eventData)["Value"]}");
    }
}