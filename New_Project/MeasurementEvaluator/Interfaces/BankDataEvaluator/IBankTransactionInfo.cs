
namespace Interfaces.BankDataEvaluator
{
    public interface IBankTransactionInfo
    {
        DateTime Time { get; }

        double Amount { get; }

        string Currency { get; }

        string Comment { get; }

        string Vendor { get; }
    }
}
