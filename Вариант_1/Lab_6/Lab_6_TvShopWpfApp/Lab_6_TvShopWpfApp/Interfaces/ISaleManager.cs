using System.Collections.Generic;
using Lab_5_TvShop.Models;

namespace Lab_6_TvShopWpfApp.Interfaces
{
    public interface ISaleManager
    {
        void SellTvs(List<Tv> tvs);
    }
}