using System.Collections.Generic;
using TvShopApp.Interfaces;

namespace TvShopApp.Models.Store
{
    public class TvStoreManager : ITvStoreManager
    {
        private readonly TvStore _tvStore;

        public TvStoreManager(TvStore tvStore)
        {
            _tvStore = tvStore;
        }

        public void AddTvCategory(TvCategory tvCategory)
        {
            _tvStore.AddTvCategory(tvCategory);
        }

        public void SellTvs(List<Tv> tvs)
        {
            _tvStore.RemoveTvsFromStore(tvs);
        }

        public IEnumerable<TvCategory> GetAllTvCategories()
        {
            return _tvStore.GetTvCategoryList();
        }
    }
}