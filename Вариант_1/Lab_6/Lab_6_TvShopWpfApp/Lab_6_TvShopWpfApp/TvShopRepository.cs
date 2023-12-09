using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab_5_TvShop.Models;
using Lab_6_TvShopWpfApp.Exceptions;
using Lab_6_TvShopWpfApp.Interfaces;
using Lab_6_TvShopWpfApp.Models;

namespace Lab_6_TvShopWpfApp
{
    public class TvShopRepository : IShopRepository
    {
        private static readonly string Directory = System.IO.Directory.GetCurrentDirectory();
        private readonly string _filePath = Path.Combine(Directory, "tvshop.txt");
        private readonly string _tvTitlesFilePath = Path.Combine(Directory, "tv_titles.txt");
        private readonly Action<string> _outputAction;

        public TvShopRepository(Action<string> outputAction)
        {
            _outputAction = outputAction;
        }

        private void Output(string message)
        {
            _outputAction?.Invoke(message);
        }

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

                Output($"Телевизор c названием {title} не найден.");
            }
            catch (Exception ex)
            {
                Output("Ошибка при десериализации: " + ex.Message);
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
                Output("Ошибка при десериализации: " + ex.Message);
                return new List<TvBox>();
            }
        }

        public void UpdateTvList(IEnumerable<TvBox> tvBoxList)
        {
            try
            {
                var json = JsonSerializer.Serialize(tvBoxList);
                File.WriteAllText(_filePath, json);
                Output("Список TvBox успешно обновлен и сохранен в файле.");
            }
            catch (Exception ex)
            {
                Output("Ошибка при обновлении списка TvBox: " + ex.Message);
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
                    Output("Телевизор успешно добавлен.");
                }
                else
                {
                    tvBoxList.Add(tvBox);
                    UpdateTvList(tvBoxList);

                    Output("Добалена новая позиция телевизоров.");
                    Output("Телевизор успешно добавлен.");
                }
            }
            catch (Exception ex)
            {
                Output("Ошибка при десериализации: " + ex.Message);
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
                Output(content);
                var storedTvBoxList = JsonSerializer.Deserialize<List<TvBox>>(content) ??
                                      new List<TvBox>();

                foreach (var tvBox in tvBoxList)
                {
                    var matchingTvBox = storedTvBoxList.Find(box => box.TvTitle.Equals(tvBox.TvTitle));
                    if (matchingTvBox != null)
                    {
                        matchingTvBox.Count++;
                        Output("Телевизор успешно добавлен.");
                    }
                    else
                    {
                        storedTvBoxList.Add(tvBox);
                        Output("Добалена новая позиция телевизоров.");
                        Output("Телевизор успешно добавлен.");
                    }
                }

                UpdateTvList(storedTvBoxList);
            }
            catch (Exception ex)
            {
                Output("Ошибка при десериализации: " + ex.Message);
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
                        if (matchingTvBox.Count > 1)
                        {
                            matchingTvBox.Count--;
                            Output($"Телевизор c названием {tv.Title} успешно удален.");
                        }
                        else
                        {
                            tvBoxList.Remove(matchingTvBox);
                            Output($"Телевизор c названием {tv.Title} успешно удален.");
                        }
                    }
                    else
                    {
                        Output($"Телевизор c названием {tv.Title} не найден.");
                    }
                }

                UpdateTvList(tvBoxList);
            }
            catch (Exception ex)
            {
                Output("Ошибка при десериализации: " + ex.Message);
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
            }
            catch (Exception ex)
            {
                Output("Ошибка при записи в файл: " + ex.Message);
            }
        }
    }
}