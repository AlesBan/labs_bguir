namespace Lab_5_ComputerPurchase.Interfaces;

public interface IPurchaseLogger
{
    void LogPurchase(IEnumerable<string> compTitleList, int purchasePrice);
}