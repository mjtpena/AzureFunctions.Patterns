using System.Collections.Generic;
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

public class ChatGPTHttpTrigger
{
    [FunctionName("StreamHttpTrigger")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
    {
        log.LogInformation("Stream results from HttpTrigger");
        
        var client = new OpenAiClient("{}");
        
        string prompt = req.Query["prompt"];

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        prompt ??= data?.prompt;
        
        var response = req.HttpContext.Response;
        response.StatusCode = 200;
        response.ContentType = "application/json-data-stream";

        await using var sw = new StreamWriter(response.Body);
        
        await foreach (var chunk in client.StreamChatCompletions(new UserMessage(prompt), maxTokens: 80))
        {
            await sw.WriteLineAsync(chunk);
        }
        await sw.FlushAsync();

        return new EmptyResult();
    }
}