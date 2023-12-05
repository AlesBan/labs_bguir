using Lab_5_TvShop.Interfaces;

namespace Lab_5_TvShop.Models
{
    public class SaleManager : IShopManager, ISaleManager
    {
        private readonly TvShopRepository _tvShopRepository;

        public SaleManager(TvShopRepository tvShopRepository)
        {
            _tvShopRepository = tvShopRepository;
        }

        public void AddTvBox(TvBox tvBox)
        {
            _tvShopRepository.AddTvBox(tvBox);
        }

        public void AddTvBoxList(List<TvBox> tvBoxList)
        {
            _tvShopRepository.AddTvBoxList(tvBoxList);
        }

        public void SellTvs(List<Tv> tvs)
        {
            _tvShopRepository.RemoveTvs(tvs);
        }

        public IEnumerable<TvBox> GetAllTvs()
        {
            return _tvShopRepository.GetTvBoxList();
        }
    }
}