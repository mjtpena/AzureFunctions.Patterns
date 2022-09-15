using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class GetMoneyBalanceFunction
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public GetMoneyBalanceFunction(ILoggerFactory loggerFactory, IMediator mediator)
    {
        _logger = loggerFactory.CreateLogger<GetMoneyBalanceFunction>();
        _mediator = mediator;
    }

    [Function(nameof(GetMoneyBalanceFunction))]
    public async Task<HttpResponseData> GetMoneyBalance([HttpTrigger(AuthorizationLevel.Function, "get", Route = "money/balance")]
    HttpRequestData req, string WalletAddress)
    {
        _logger.LogInformation("Started to query user's balance...");

        var query = new MoneyBalanceQuery
        {
            WalletAddress = WalletAddress
        };

        var result = await _mediator.Send(query);
        var response = req.CreateResponse();

        if (result == null)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
        }

        await response.WriteAsJsonAsync(result);
        return response;
    }
}