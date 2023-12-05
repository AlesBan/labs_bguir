namespace Lab_5_ComputerPurchase.Models;

public class ComputerBox
{
    public string CompTitle { get; set; } = string.Empty;
    public int Price { get; set; }
    public int Count { get; set; }
    public string Info() => $"Title: {CompTitle}, Price: {Price}, Count: {Count}";
}