namespace Laba_5_TvSales.Models
{
    public class TvCategory
    {
        public string Title { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public string GetFullInfo() => $"Title: {Title}, Price: {Price}, Count: {Count}";
    }
}