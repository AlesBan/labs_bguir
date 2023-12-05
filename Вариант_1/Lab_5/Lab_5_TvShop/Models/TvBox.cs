namespace Lab_5_TvShop.Models
{
    public class TvBox
    {
        public string TvTitle { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Count { get; set; }
        public string Info() => $"Title: {TvTitle}, Price: {Price}, Count: {Count}";
    }
}