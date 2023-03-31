using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenAI;
using OpenAI.Models.ChatCompletion;

namespace ChatGPT;

public class SimpleHttpTrigger
{
    [FunctionName("SimpleHttpTrigger")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        log.LogInformation("Simple HTTP trigger that returns a complete response");

        var client = new OpenAiClient("{}");

        string prompt = req.Query["prompt"];

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        prompt ??= data?.prompt;

        var response = await client.GetChatCompletions(new UserMessage(prompt), maxTokens: 80);

        return response != null
            ? new OkObjectResult(response)
            : new BadRequestObjectResult("Please pass a prompt on the query string or in the request body");

    }
}