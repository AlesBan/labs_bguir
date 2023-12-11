using System;
using System.Collections.Generic;
using System.IO;
using TvShopApp.Helpers;

namespace TvShopApp
{
    public class TvStoreLogger
    {
        private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
        private readonly string _filePath = Path.Combine(Directory, "tvStore_Logs.txt");

        public void LogSale(IEnumerable<string> tvTitleList, double salePrice)
        {
            var logEntry =
                $"Sale Date: {DateTime.Now}, Sale Amount: {salePrice}, TVs: {string.Join(", ", tvTitleList)}";

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

                Console.WriteLine("Sale information successfully written to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing sale information to the file: {ex.Message}");
            }
        }
    }
}