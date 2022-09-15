using MediatR;

public class MoneyTransferCommand :
    IRequest<MoneyTransferCommandResult>
{
    public string? FromWalletAddress { get; set; }
    public string? ToWalletAddress { get; set; }
    public decimal Amount { get; set; }
}