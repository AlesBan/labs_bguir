using Lab_5_ComputerPurchase.Exceptions;
using Lab_5_ComputerPurchase.Interfaces;

namespace Lab_5_ComputerPurchase.Logging;

public class FilePurchaseLogger : IPurchaseLogger
{
    private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
    private readonly string _filePath = Path.Combine(Directory, "compPurchase_logs.txt");

    public void LogPurchase(IEnumerable<string> compTitleList, int purchasePrice)
    {
        var logEntry =
            $"Дата закупки: {DateTime.Now}, Сумма закупки: {purchasePrice}, Компьютеры: {string.Join(", ", compTitleList)}";

        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            using (var writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(logEntry);
            }

            Console.WriteLine("Информация о закупке успешно записана в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи информации о продаже в файл: {ex.Message}");
        }
    }

}