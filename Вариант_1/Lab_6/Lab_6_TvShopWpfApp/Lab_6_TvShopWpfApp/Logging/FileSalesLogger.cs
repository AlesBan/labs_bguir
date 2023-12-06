using System;
using System.Collections.Generic;
using System.IO;
using Lab_6_TvShopWpfApp.Exceptions;
using Lab_6_TvShopWpfApp.Interfaces;

namespace Lab_6_TvShopWpfApp.Logging;

public class FileSalesLogger : ISalesLogger
{
    private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
    private readonly string _filePath = Path.Combine(Directory, "tvshop_logs.txt");

    public void LogSale(List<string> tvTitleList, int salePrice)
    {
        var logEntry =
            $"Дата продажи: {DateTime.Now}, Сумма продажи: {salePrice}, Телевизоры: {string.Join(", ", tvTitleList)}";

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

            Console.WriteLine("Информация о продаже успешно записана в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи информации о продаже в файл: {ex.Message}");
        }
    }
}