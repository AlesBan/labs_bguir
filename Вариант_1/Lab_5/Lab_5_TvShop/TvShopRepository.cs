using System.Text.Json;
using Lab_5_TvShop.Exceptions;
using Lab_5_TvShop.Interfaces;
using Lab_5_TvShop.Models;

namespace Lab_5_TvShop
{
    public class TvShopRepository : IShopRepository
    {
        private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
        private readonly string _filePath = Path.Combine(Directory, "tvshop.txt");
        private readonly string _tvTitlesFilePath = Path.Combine(Directory, "tv_titles.txt");

        public TvBox? GetTvBoxByTitle(string title)
        {
            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var content = File.ReadAllText(_filePath);
                var tvBoxList = JsonSerializer.Deserialize<List<TvBox>>(content) ?? new List<TvBox>();
                var tvBox = tvBoxList.FirstOrDefault(tvBox
                    => tvBox.TvTitle.Equals(title, StringComparison.OrdinalIgnoreCase));

                if (tvBox != null)
                {
                    return tvBox;
                }

                Console.WriteLine($"Телевизор c названием {title} не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при десериализации: " + ex.Message);
            }

            return null;
        }

        public IEnumerable<TvBox> GetTvBoxList()
        {
            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var content = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TvBox>>(content) ?? new List<TvBox>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при десериализации: " + ex.Message);
                return new List<TvBox>();
            }
        }

        public void UpdateTvList(IEnumerable<TvBox> tvBoxList)
        {
            try
            {
                var json = JsonSerializer.Serialize(tvBoxList);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Список TvBox успешно обновлен и сохранен в файле.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при обновлении списка TvBox: " + ex.Message);
            }
        }

        public void AddTvBox(TvBox tvBox)
        {
            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var content = File.ReadAllText(_filePath);
                var tvBoxList = JsonSerializer.Deserialize<List<TvBox>>(content) ??
                                new List<TvBox>();

                var matchingTvBox = tvBoxList.Find(box => box.TvTitle.Equals(tvBox.TvTitle));
                if (matchingTvBox != null)
                {
                    matchingTvBox.Count++;
                    Console.WriteLine("Телевизор успешно добавлен.");
                }
                else
                {
                    tvBoxList.Add(tvBox);
                    UpdateTvList(tvBoxList);

                    Console.WriteLine("Добалена новая позиция телевизоров.");
                    Console.WriteLine("Телевизор успешно добавлен.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при десериализации: " + ex.Message);
            }
        }

        public void AddTvBoxList(List<TvBox> tvBoxList)
        {
            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var content = File.ReadAllText(_filePath);
                Console.WriteLine(content);
                var storedTvBoxList = JsonSerializer.Deserialize<List<TvBox>>(content) ??
                                      new List<TvBox>();

                foreach (var tvBox in tvBoxList)
                {
                    var matchingTvBox = storedTvBoxList.Find(box => box.TvTitle.Equals(tvBox.TvTitle));
                    if (matchingTvBox != null)
                    {
                        matchingTvBox.Count++;
                        Console.WriteLine("Телевизор успешно добавлен.");
                    }
                    else
                    {
                        storedTvBoxList.Add(tvBox);
                        Console.WriteLine("Добалена новая позиция телевизоров.");
                        Console.WriteLine("Телевизор успешно добавлен.");
                    }
                }

                UpdateTvList(storedTvBoxList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при десериализации: " + ex.Message);
            }
        }

        public void RemoveTvs(List<Tv> tvs)
        {
            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var content = File.ReadAllText(_filePath);
                var tvBoxList = JsonSerializer.Deserialize<List<TvBox>>(content) ?? new List<TvBox>();

                foreach (var tv in tvs)
                {
                    var matchingTvBox = tvBoxList.Find(box => box.TvTitle.Equals(tv.Title));
                    if (matchingTvBox != null)
                    {
                        matchingTvBox.Count--;
                        Console.WriteLine($"Телевизор {matchingTvBox.TvTitle} успешно удален.");

                        if (matchingTvBox.Count != 0)
                        {
                            continue;
                        }

                        tvBoxList.Remove(matchingTvBox);
                        Console.WriteLine($"Категория телевизоров {matchingTvBox.TvTitle} удалена.");
                    }
                    else
                    {
                        Console.WriteLine($"Телевизор c названием {tv.Title} не найден.");
                    }
                }

                UpdateTvList(tvBoxList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при десериализации: " + ex.Message);
            }
        }

        public void ParseTvTitlesAndAddTvBoxes()
        {
            if (!File.Exists(_tvTitlesFilePath))
            {
                throw new SourceFileNotFoundException(_tvTitlesFilePath);
            }

            if (!File.Exists(_filePath))
            {
                throw new SourceFileNotFoundException(_filePath);
            }

            try
            {
                var tvTitles = File.ReadAllLines(_tvTitlesFilePath);
                var random = new Random();

                var tvBoxList = tvTitles.Select(t =>
                    new TvBox
                    {
                        TvTitle = t.Trim(),
                        Price = random.Next(100, 1000),
                        Count = random.Next(1, 10)
                    }).ToList();

                var json = JsonSerializer.Serialize(tvBoxList);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Объекты TvBox успешно записаны в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при записи в файл: " + ex.Message);
            }
        }
    }
}