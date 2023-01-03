using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace RedisCachePatterns.Functions;

public class WriteThroughFunction
{
    private readonly IDistributedCache _redisCache;

    public WriteThroughFunction(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    [Function("WriteThroughFunction")]
    public async Task<CacheResponse> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] CacheRequest request,
        FunctionContext executionContext, CancellationToken cancellationToken)
    {
        var logger = executionContext.GetLogger("WriteThroughFunction");
        logger.LogInformation("Starting Write-Through Function.");

        // Write the value to the cache and the source
        await _redisCache.SetStringAsync(request.Key, request.ValueToInsert, token: cancellationToken);
        await WriteValueToDatabase(request.Key, request.ValueToInsert);

        // Return the value to the client
        return new CacheResponse
        {
            Message = "Value written to cache and source"
        };
    }

    private async Task WriteValueToDatabase(string requestKey, string requestValueToInsert)
    {
        // Perform an insert operation to your database
    }
}