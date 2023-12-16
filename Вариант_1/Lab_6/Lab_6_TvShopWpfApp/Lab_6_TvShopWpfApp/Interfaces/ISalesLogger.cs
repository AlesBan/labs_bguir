using System.Collections.Generic;

namespace Lab_6_TvShopWpfApp.Interfaces;

public interface ISalesLogger
{
    void LogSale(List<string> tvTitleList, int salePrice);
}