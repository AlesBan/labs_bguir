using System;
using System.IO;

namespace TvShopApp.Helpers
{
    public static class FileHelper
    {
        public static void NoFileReport_KillProcess(string filePath)
        {
            Console.WriteLine($"File {filePath} not found.");
            File.Create(filePath);
            Console.WriteLine("An empty file was created.");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}