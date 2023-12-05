using System.Collections.Generic;
using Laba_5_TvSales.Models;

namespace Laba_5_TvSales.Interfaces
{
    public interface ITvStoreManager
    {
        void AddTvCategory(TvCategory tvCategory);
        void SellTvs(List<Tv> tvs);
        IEnumerable<TvCategory> GetAllTvCategories();
    }
}