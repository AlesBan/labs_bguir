using System.Collections.Generic;
using Lab_5_TvShop.Models;

namespace Lab_6_TvShopWpfApp.Interfaces
{
    public interface IShopManager
    {
        void AddTvBox(TvBox tvBox);
        void AddTvBoxList(List<TvBox> tvBoxList);
    }
}