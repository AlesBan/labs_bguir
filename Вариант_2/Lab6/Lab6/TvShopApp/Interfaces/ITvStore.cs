using System.Collections.Generic;
using TvShopApp.Models;

namespace TvShopApp.Interfaces
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