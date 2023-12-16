using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using TvShopApp.Helpers;
using TvShopApp.Interfaces;

namespace TvShopApp.Models.Store
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

            MessageBox.Show($"TV with the title {title} was not found.");
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
                MessageBox.Show("TV successfully added.");
            }
            else
            {
                tvCategoryList.Add(tvCategory);
                SaveTvCategories(tvCategoryList);
                MessageBox.Show("Added a new TV category.\nTV successfully added.");
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
                }
                else
                {
                    storedTvCategoryList.Add(tvCategory);
                    MessageBox.Show("Added a new TV category.\nTV successfully added.");
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

                    if (existingTvCategory.Count != 0)
                    {
                        continue;
                    }

                    tvCategoryList.Remove(existingTvCategory);
                    MessageBox.Show($"TV category {existingTvCategory.Title} removed.");
                }
                else
                {
                    MessageBox.Show($"TV with the title {tv.Title} not found.");
                }
            }

            SaveTvCategories(tvCategoryList);
        }

        public void InitializeTvCategories()
        {
            if (!File.Exists(_customTvTitlesFilePath) || !File.Exists(_tvStorageFilePath))
            {
                MessageBox.Show("One or both of the required files are missing.");
                return;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing TvCategory list: " + ex.Message);
            }
        }

        private List<TvCategory> LoadTvCategories()
        {
            if (!File.Exists(_tvStorageFilePath))
            {
                MessageBox.Show("The TV storage file is missing.");
                return new List<TvCategory>();
            }

            try
            {
                var content = File.ReadAllText(_tvStorageFilePath);
                return JsonSerializer.Deserialize<List<TvCategory>>(content) ?? new List<TvCategory>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Deserialization error: " + ex.Message);
                return new List<TvCategory>();
            }
        }

        private void SaveTvCategories(List<TvCategory> tvCategoryList)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json= JsonSerializer.Serialize(tvCategoryList, options);
                File.WriteAllText(_tvStorageFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating TvCategory list: " + ex.Message);
            }
        }
    }
}