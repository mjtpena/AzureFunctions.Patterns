using Microsoft.Azure.Functions.Worker;  
using Microsoft.Extensions.Caching.Distributed;  
using Microsoft.Extensions.Logging;  
  
namespace RedisCachePatterns.Functions;  
  
public class WriteBehindFunction  
{  
    private readonly IDistributedCache _redisCache;  
  
    public WriteBehindFunction(IDistributedCache redisCache)  
    {  
        _redisCache = redisCache;  
    }  
  
    [Function("WriteBehindFunction")]  
    public async Task<CacheResponse> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] CacheRequest request,  
        FunctionContext executionContext, CancellationToken cancellationToken)  
    {  
        var logger = executionContext.GetLogger("WriteBehindFunction");  
        logger.LogInformation("Starting Write-Behind Function.");  
  
        // Write the value to the cache (Synchronously)  
        _redisCache.SetString(request.Key, request.ValueToInsert);  
                  
        // Asynchronously update the source  
        await PublishToEventHubsAsync(request.Key, request.ValueToInsert);  
  
        return new CacheResponse  
        {  
            Message = "Value written to cache and update to source scheduled"  
        };  
    }  
  
    private async Task PublishToEventHubsAsync(string requestKey, string requestValueToInsert)  
    {  
        // Schedule an insert operation to your database  
    }  
}