using MediatR;

public class MoneyBalanceQuery :
    IRequest<MoneyBalanceResult>
{
    public string? WalletAddress { get; set; }
}