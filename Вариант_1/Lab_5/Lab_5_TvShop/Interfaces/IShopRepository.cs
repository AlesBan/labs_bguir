using System.Collections.Generic;
using Lab_5_TvShop.Models;

namespace Lab_5_TvShop.Interfaces
{
    public interface IShopRepository
    {
        TvBox? GetTvBoxByTitle(string title);
        IEnumerable<TvBox> GetTvBoxList();
        void UpdateTvList(IEnumerable<TvBox> tvBoxList);
        void AddTvBox(TvBox tvBox);
        void AddTvBoxList(List<TvBox> tvBoxList);
        void RemoveTvs(List<Tv> tvs);
    }
}