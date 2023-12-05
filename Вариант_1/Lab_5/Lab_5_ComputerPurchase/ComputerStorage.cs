using System.Text.Json;
using Lab_5_ComputerPurchase.Exceptions;
using Lab_5_ComputerPurchase.Interfaces;
using Lab_5_ComputerPurchase.Models;

namespace Lab_5_ComputerPurchase;

public class ComputerStorage : IComputerStorage
{
    private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
    private readonly string _filePath = Path.Combine(Directory, "compStore.txt");
    private readonly string _computerTitlesFilePath = Path.Combine(Directory, "comp_titles.txt");

    public ComputerBox? GetComputerBoxByTitle(string title)
    {
        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            var content = File.ReadAllText(_filePath);
            var compBoxList = JsonSerializer.Deserialize<List<ComputerBox>>(content) ?? new List<ComputerBox>();
            var compBox = compBoxList.FirstOrDefault(compBox
                => compBox.CompTitle.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (compBox != null)
            {
                return compBox;
            }

            Console.WriteLine($"Компьютер c названием {title} не найден.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при десериализации: " + ex.Message);
        }

        return null;
    }

    public IEnumerable<ComputerBox> GetComputerBoxList()
    {
        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            var content = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ComputerBox>>(content) ?? new List<ComputerBox>();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при десериализации: " + ex.Message);
            return new List<ComputerBox>();
        }
    }

    public void UpdateComputerList(IEnumerable<ComputerBox> computerBoxList)
    {
        try
        {
            var json = JsonSerializer.Serialize(computerBoxList);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Список computerBoxList успешно обновлен и сохранен в файле.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при обновлении списка ComputerBoxList: " + ex.Message);
        }
    }

    public void AddComputerList(List<Computer> computerList)
    {
        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            var content = File.ReadAllText(_filePath);
            var compBoxList = JsonSerializer.Deserialize<List<ComputerBox>>(content) ?? new List<ComputerBox>();

            foreach (var computer in computerList)
            {
                var matchingCompBox = compBoxList.Find(box => box.CompTitle.Equals(computer.Title));
                if (matchingCompBox != null)
                {
                    matchingCompBox.Count++;
                    Console.WriteLine($"Компьютер {matchingCompBox.CompTitle} успешно добавлен.");
                }
                else
                {
                    var newCompBox = new ComputerBox()
                    {
                        CompTitle = computer.Title,
                        Count = 1
                    };
                    compBoxList.Add(newCompBox);
                    Console.WriteLine($"Компьютер {newCompBox.CompTitle} успешно добавлен в новую категорию.");
                }
            }

            UpdateComputerList(compBoxList);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при десериализации: " + ex.Message);
        }
    }

    public void RemoveComputers(List<Computer> computers)
    {
        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            var content = File.ReadAllText(_filePath);
            var computerBoxList = JsonSerializer.Deserialize<List<ComputerBox>>(content) ?? new List<ComputerBox>();

            foreach (var computer in computers)
            {
                var matchingComputerBox = computerBoxList.Find(box => box.CompTitle.Equals(computer.Title));
                if (matchingComputerBox != null)
                {
                    matchingComputerBox.Count--;
                    Console.WriteLine($"Компьютер {matchingComputerBox.CompTitle} успешно удален.");

                    if (matchingComputerBox.Count != 0)
                    {
                        continue;
                    }

                    computerBoxList.Remove(matchingComputerBox);
                    Console.WriteLine($"Категория компьютеров {matchingComputerBox.CompTitle} удалена.");
                }
                else
                {
                    Console.WriteLine($"Компьютер c названием {computer.Title} не найден.");
                }
            }

            UpdateComputerList(computerBoxList);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при десериализации: " + ex.Message);
        }
    }

    public void ParseCompTitlesAndAddCompBoxes()
    {
        if (!File.Exists(_computerTitlesFilePath))
        {
            throw new SourceFileNotFoundException(_computerTitlesFilePath);
        }

        if (!File.Exists(_filePath))
        {
            throw new SourceFileNotFoundException(_filePath);
        }

        try
        {
            var tvTitles = File.ReadAllLines(_computerTitlesFilePath);
            var random = new Random();

            var tvBoxList = tvTitles.Select(t =>
                new ComputerBox
                {
                    CompTitle = t.Trim(),
                    Price = random.Next(100, 1000),
                    Count = random.Next(1, 10)
                }).ToList();

            var json = JsonSerializer.Serialize(tvBoxList);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Объекты ComputerBox успешно записаны в файл.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при записи в файл: " + ex.Message);
        }
    }
}