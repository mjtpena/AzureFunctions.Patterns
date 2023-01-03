using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace RedisCachePatterns.Functions;

public class CacheAsideFunction
{
    private readonly IDistributedCache _redisCache;

    public CacheAsideFunction(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    [Function("CacheAsideFunction")]
    public async Task<CacheResponse> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] CacheRequest request,
        FunctionContext executionContext, CancellationToken cancellationToken)
    {
        var logger = executionContext.GetLogger("CacheAsideFunction");
        logger.LogInformation("Starting Cache-Aside Function.");

        // Get the requested key
        var cacheKey = request.Key;
        
        // Get the cache value
        var cacheValue = await _redisCache.GetStringAsync(cacheKey, token: cancellationToken);

        // If the key is not present in the cache, retrieve it from the source and store it in the cache
        if (cacheValue == null)
        {
            cacheValue = await GetValueFromDatabase(cacheKey);
            await _redisCache.SetStringAsync(cacheKey, cacheValue, token: cancellationToken);
        }

        // Return the value to the client
        return new CacheResponse
        {
            CacheValue = cacheValue,
            Message = "Success"
        };
    }

    private async Task<string?> GetValueFromDatabase(string cacheKey)
    {
        return "Value from Database";
    }
}