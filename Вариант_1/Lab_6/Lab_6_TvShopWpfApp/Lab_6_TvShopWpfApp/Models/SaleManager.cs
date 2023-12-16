using System.Collections.Generic;
using Lab_5_TvShop;
using Lab_5_TvShop.Models;
using Lab_6_TvShopWpfApp.Interfaces;

namespace Lab_6_TvShopWpfApp.Models
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