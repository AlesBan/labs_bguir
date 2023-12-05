namespace Lab_5_TvShop.Interfaces;

public interface ISalesLogger
{
    void LogSale(List<string> tvTitleList, int salePrice);
}