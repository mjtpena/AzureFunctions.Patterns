using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobTrigger.Functions;

public class BlobTriggerFunction
{
    [Function("BlobTriggerFunction")]
    public async Task Run(
        [BlobTrigger("test-samples-trigger/{name}")] string myBlob, string blobTrigger,
        FunctionContext context)
    {
        var logger = context.GetLogger("BlobFunction");
        logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{blobTrigger} ");
    }
}