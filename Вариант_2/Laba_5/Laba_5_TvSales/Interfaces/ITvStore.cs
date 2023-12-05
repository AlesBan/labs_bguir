using System.Collections.Generic;
using Laba_5_TvSales.Models;

namespace Laba_5_TvSales.Interfaces
{
    public interface ITvStore
    {
        TvCategory GetTvCategory(string title);
        IEnumerable<TvCategory> GetTvCategoryList();
        void AddTvCategory(TvCategory tvCategory);
        void AddTvCategoryList(List<TvCategory> tvCategoryList);
        void RemoveTvsFromStore(List<Tv> tvs);
    }
}