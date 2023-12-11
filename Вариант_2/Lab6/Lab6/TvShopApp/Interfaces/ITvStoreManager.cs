using System.Collections.Generic;
using TvShopApp.Models;

namespace TvShopApp.Interfaces
{
    public interface ITvStoreManager
    {
        void AddTvCategory(TvCategory tvCategory);
        void SellTvs(List<Tv> tvs);
        IEnumerable<TvCategory> GetAllTvCategories();
    }
}