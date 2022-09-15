using System.Net;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class MoneyTransferFunction
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public MoneyTransferFunction(ILoggerFactory loggerFactory, IMediator mediator)
    {
        _logger = loggerFactory.CreateLogger<MoneyTransferFunction>();
        _mediator = mediator;
    }

    [Function(nameof(MoneyTransferFunction))]
    public async Task<HttpResponseData> PostMoneyTransfer([HttpTrigger(AuthorizationLevel.Function, "post", Route = "money/transfer")]
    HttpRequestData req)
    {
        _logger.LogInformation("Started to money transfer command...");

        var command = new MoneyTransferCommand
        {
            FromWalletAddress = "0x1...",
            ToWalletAddress = "0x2...",
            Amount = 777
        };

        var result = await _mediator.Send(command);
        var response = req.CreateResponse();

        if (result.Success)
            _logger.LogInformation(result.Message);
        else
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            _logger.LogError(result.Message);
        }

        await response.WriteAsJsonAsync(result);
        return response;
    }
}