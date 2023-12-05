using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Laba_5_TvSales.Helpers;
using Laba_5_TvSales.Interfaces;

namespace Laba_5_TvSales.Models.Store
{
    public class TvStore : ITvStore
    {
        private readonly string _tvStorageFilePath;
        private readonly string _customTvTitlesFilePath;

        public TvStore(string tvStorageFilePath, string customTvTitlesFilePath)
        {
            _tvStorageFilePath = tvStorageFilePath;
            _customTvTitlesFilePath = customTvTitlesFilePath;
        }

        public TvCategory GetTvCategory(string title)
        {
            var tvCategoryList = LoadTvCategories();
            var tvCategory =
                tvCategoryList.FirstOrDefault(tv => tv.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (tvCategory != null)
            {
                return tvCategory;
            }

            Console.WriteLine($"TV with the title {title} was not found.");
            return null;
        }

        public IEnumerable<TvCategory> GetTvCategoryList()
        {
            return LoadTvCategories();
        }
        public void AddTvCategory(TvCategory tvCategory)
        {
            var tvCategoryList = LoadTvCategories();
            var existingTvCategory = tvCategoryList.Find(category => category.Title.Equals(tvCategory.Title));

            if (existingTvCategory != null)
            {
                existingTvCategory.Count++;
                Console.WriteLine("TV successfully added.");
            }
            else
            {
                tvCategoryList.Add(tvCategory);
                SaveTvCategories(tvCategoryList);
                Console.WriteLine("Added a new TV category.");
                Console.WriteLine("TV successfully added.");
            }
        }

        public void AddTvCategoryList(List<TvCategory> tvCategoryList)
        {
            var storedTvCategoryList = LoadTvCategories();

            foreach (var tvCategory in tvCategoryList)
            {
                var existingTvCategory = storedTvCategoryList.FirstOrDefault(category
                    => category.Title.Equals(tvCategory.Title));

                if (existingTvCategory != null)
                {
                    existingTvCategory.Count++;
                    Console.WriteLine("TV successfully added.");
                }
                else
                {
                    storedTvCategoryList.Add(tvCategory);
                    Console.WriteLine("Added a new TV category.");
                    Console.WriteLine("TV successfully added.");
                }
            }

            SaveTvCategories(storedTvCategoryList);
        }

        public void RemoveTvsFromStore(List<Tv> tvs)
        {
            var tvCategoryList = LoadTvCategories();

            foreach (var tv in tvs)
            {
                var existingTvCategory = tvCategoryList.Find(category => category.Title.Equals(tv.Title));

                if (existingTvCategory != null)
                {
                    existingTvCategory.Count--;
                    Console.WriteLine($"TV {existingTvCategory.Title} successfully removed.");

                    if (existingTvCategory.Count != 0)
                    {
                        continue;
                    }

                    tvCategoryList.Remove(existingTvCategory);
                    Console.WriteLine($"TV category {existingTvCategory.Title} removed.");
                }
                else
                {
                    Console.WriteLine($"TV with the title {tv.Title} not found.");
                }
            }

            SaveTvCategories(tvCategoryList);
        }

        public void InitializeTvCategories()
        {
            if (!File.Exists(_customTvTitlesFilePath))
            {
                FileHelper.NoFileReport_KillProcess(_customTvTitlesFilePath);
            }

            if (!File.Exists(_tvStorageFilePath))
            {
                FileHelper.NoFileReport_KillProcess(_tvStorageFilePath);
            }

            try
            {
                var customTvTitles = File.ReadAllLines(_customTvTitlesFilePath);
                var random = new Random();

                var customTvCategoriesList = new List<TvCategory>();

                foreach (var t in customTvTitles)
                {
                    var tvCategory = new TvCategory
                    {
                        Title = t.Trim(),
                        Price = random.Next(100, 1000),
                        Count = random.Next(1, 10)
                    };

                    customTvCategoriesList.Add(tvCategory);
                }

                SaveTvCategories(customTvCategoriesList);
                Console.WriteLine("TvCategory list successfully initialized.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing TvCategory list: " + ex.Message);
            }
        }

        private List<TvCategory> LoadTvCategories()
        {
            if (!File.Exists(_tvStorageFilePath))
            {
                FileHelper.NoFileReport_KillProcess(_tvStorageFilePath);
            }

            try
            {
                var content = File.ReadAllText(_tvStorageFilePath);
                return JsonSerializer.Deserialize<List<TvCategory>>(content) ?? new List<TvCategory>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);
                return new List<TvCategory>();
            }
        }

        private void SaveTvCategories(List<TvCategory> tvCategoryList)
        {
            try
            {
                var json = JsonSerializer.Serialize(tvCategoryList);
                File.WriteAllText(_tvStorageFilePath, json);
                Console.WriteLine("TvCategory list successfully updated and saved to a file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating TvCategory list: " + ex.Message);
            }
        }
    }
}