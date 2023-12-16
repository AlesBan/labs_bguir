namespace Lab_6_TvShopWpfApp.Models
{
    public class TvBox
    {
        public string TvTitle { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Count { get; set; }
        public string Info() => $"Название: {TvTitle}, Цена: {Price}, Кол-во: {Count}";
    }
}