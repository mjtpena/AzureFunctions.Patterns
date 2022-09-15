
using MediatR;
public class MoneyTransferCommandHandler :
    IRequestHandler<MoneyTransferCommand, MoneyTransferCommandResult>
{
    public async Task<MoneyTransferCommandResult> Handle(MoneyTransferCommand request,
        CancellationToken cancellationToken)
    {
        //Command Operations: Save to DB, Send to Event Hubs, Call another HTTP Post
        await Task.Delay(1000);
        
        return new MoneyTransferCommandResult{
            Success = true,
            Message = "Transfer successful with 0x123123123 hash"
        };
    }
}