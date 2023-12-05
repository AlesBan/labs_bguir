using Lab_5_TvShop.Models;

namespace Lab_5_TvShop.Interfaces
{
    public interface IShopManager
    {
        void AddTvBox(TvBox tvBox);
        void AddTvBoxList(List<TvBox> tvBoxList);
    }
}