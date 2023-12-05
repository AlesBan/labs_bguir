using System;
using System.Collections.Generic;
using System.IO;
using Laba_5_ByingComps.Helpers;
using Laba_5_ByingComps.Interfaces;

namespace Laba_5_ByingComps
{
    public class FileLogger : IFileLogger
    {
        private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
        private readonly string _filePath = Path.Combine(Directory, "compStore_logs.txt");

        public void Log(IEnumerable<string> compTitleList, int purchasePrice)
        {
            var logEntry =
                $"Дата закупки: {DateTime.Now}, Сумма закупки: {purchasePrice}, Компьютеры: {string.Join(", ", compTitleList)}";

            if (!File.Exists(_filePath))
            {
                FileHelper.NoFileReport_KillProcess(_filePath);
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
}