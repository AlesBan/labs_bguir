namespace Lab_5_ComputerPurchase.Exceptions;

public class SourceFileNotFoundException : Exception
{
    public SourceFileNotFoundException(string filePath) : base($"Файл {filePath} не найден!")
    {
        File.Create(filePath);
        Console.WriteLine("Создан пустой файл.");
    }
}