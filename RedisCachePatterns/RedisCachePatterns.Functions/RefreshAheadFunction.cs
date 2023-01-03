using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace RedisCachePatterns.Functions;

public class RefreshAheadFunction
{
    private readonly IDistributedCache _redisCache;

    public RefreshAheadFunction(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }
    
    [Function("HttpRefreshAheadFunction")]
    public async Task<CacheResponse> Get([HttpTrigger(AuthorizationLevel.Function, "get", "post")] CacheRequest request,
        FunctionContext executionContext, CancellationToken cancellationToken)
    {
        var logger = executionContext.GetLogger("HttpRefreshAheadFunction");
        logger.LogInformation("Starting HTTP Refresh-Ahead Function.");

        // Get the requested key
        var cacheKey = request.Key;
        
        // Get the cache value. It is guaranteed that there is a value here because of the timer refresh.
        var cacheValue = await _redisCache.GetStringAsync(cacheKey, token: cancellationToken);

        // Return the value to the client
        return new CacheResponse
        {
            CacheValue = cacheValue,
            Message = "Success"
        };
    }
    
    [Function("TimerRefreshAheadFunction")]
    public async Task<CacheResponse> Refresh([TimerTrigger("0 */5 * * * *")] CacheRequest request, 
        FunctionContext context, CancellationToken cancellationToken)
    {
        var logger = context.GetLogger("TimerRefreshAheadFunction");
        logger.LogInformation("Starting Timer for Refresh-Ahead Function.");

        // Get the requested key
        var cacheKey = request.Key;
        
        // Get the cache value
        var cacheValue = await _redisCache.GetStringAsync(cacheKey, token: cancellationToken);
        // If cache value is not set, retrieve from original source and store in cache
        if (string.IsNullOrEmpty(cacheValue))
        {
            string value = await GetValueFromOriginalSourceAsync();
            cacheValue = value;
            await _redisCache.SetStringAsync(request.Key, request.ValueToReturn, token: cancellationToken);
        }

        // Return the value to the client
        return new CacheResponse
        {
            CacheValue = cacheValue,
            Message = "Success"
        };
    }

    private static async Task<string> GetValueFromOriginalSourceAsync()
    {
        return "Retrieve data from original source (e.g., CosmosDB database)";
    }

    private static async Task SetCacheValueAsync(string cacheName, string key, string value)
    {
        // Store value in cache
    }

}