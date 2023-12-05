namespace Laba_5_ByingComps.Models
{
    public class ComputerCategory
    {
        public string CompTitle { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Count { get; set; }
        public string FullInfo() => $"Title: {CompTitle}, Price: {Price}, Count: {Count}";
    }
};

