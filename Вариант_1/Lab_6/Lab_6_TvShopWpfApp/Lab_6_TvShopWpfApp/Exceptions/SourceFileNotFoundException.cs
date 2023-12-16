using System;
using System.IO;

namespace Lab_6_TvShopWpfApp.Exceptions;

public class SourceFileNotFoundException : Exception
{
    public SourceFileNotFoundException(string filePath) : base($"Файл {filePath} не найден!")
    {
        File.Create(filePath);
        Console.WriteLine("Создан пустой файл.");
    }
}