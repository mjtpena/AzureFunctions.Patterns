
using MediatR;
public class MoneyBalanceQueryHandler :
    IRequestHandler<MoneyBalanceQuery, MoneyBalanceResult>
{
    public async Task<MoneyBalanceResult> Handle(MoneyBalanceQuery request,
        CancellationToken cancellationToken)
    {
        //DB Operations or other external services to fetch data
        await Task.Delay(1000);
        
        return new MoneyBalanceResult{
            WalletAddress = request.WalletAddress,
            Balance = 7777
        };
    }
}