using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Laba_5_ByingComps.Helpers;
using Laba_5_ByingComps.Interfaces;
using Laba_5_ByingComps.Models;

namespace Laba_5_ByingComps
{
    public class ComputerStorage : IComputerStorage
    {
        private readonly string _filePath;
        private readonly string _computerTitlesFilePath;

        public ComputerStorage(string filePath, string computerTitlesFilePath)
        {
            _filePath = filePath;
            _computerTitlesFilePath = computerTitlesFilePath;
        }

        private string ReadFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FileHelper.NoFileReport_KillProcess(filePath);
            }

            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading file: " + ex.Message);
            }
        }

        private void WriteFileContent(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                throw new Exception("Error writing to file: " + ex.Message);
            }
        }

        public ComputerCategory? GetComputerCategoryByTitle(string title)
        {
            var content = ReadFileContent(_filePath);
            var compCategoryList = JsonSerializer.Deserialize<List<ComputerCategory>>(content) ??
                                   new List<ComputerCategory>();

            var compCategory = compCategoryList.FirstOrDefault(compCategory
                => compCategory.CompTitle.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (compCategory != null)
            {
                return compCategory;
            }

            throw new Exception($"Computer with the title {title} was not found.");
        }

        public IEnumerable<ComputerCategory> GetComputerCategoryList()
        {
            var content = ReadFileContent(_filePath);
            return JsonSerializer.Deserialize<List<ComputerCategory>>(content) ?? Enumerable.Empty<ComputerCategory>();
        }

        public void UpdateComputerList(IEnumerable<ComputerCategory> computerCategoryList)
        {
            try
            {
                var json = JsonSerializer.Serialize(computerCategoryList);
                WriteFileContent(_filePath, json);
                Console.WriteLine("The computerCategoryList has been successfully updated and saved to the file.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the ComputerCategoryList: " + ex.Message);
            }
        }

        public void AddComputerList(List<Computer> computerList)
        {
            var content = ReadFileContent(_filePath);
            var compCategoryList = JsonSerializer.Deserialize<List<ComputerCategory>>(content) ??
                                   new List<ComputerCategory>();

            foreach (var computer in computerList)
            {
                var matchingCompCategory = compCategoryList.Find(category => category.CompTitle.Equals(computer.Title));
                if (matchingCompCategory != null)
                {
                    matchingCompCategory.Count++;
                    Console.WriteLine($"Computer {matchingCompCategory.CompTitle} has been successfully added.");
                }
                else
                {
                    var newCompCategory = new ComputerCategory()
                    {
                        CompTitle = computer.Title,
                        Count = 1
                    };
                    compCategoryList.Add(newCompCategory);
                    Console.WriteLine(
                        $"Computer {newCompCategory.CompTitle} has been successfully added to a new category.");
                }
            }

            UpdateComputerList(compCategoryList);
        }

        public void RemoveComputers(List<Computer> computers)
        {
            var content = ReadFileContent(_filePath);
            var computerCategoryList = JsonSerializer.Deserialize<List<ComputerCategory>>(content) ??
                                       new List<ComputerCategory>();

            foreach (var computer in computers)
            {
                var matchingComputerCategory =
                    computerCategoryList.Find(category => category.CompTitle.Equals(computer.Title));
                if (matchingComputerCategory != null)
                {
                    matchingComputerCategory.Count--;
                    Console.WriteLine($"Computer {matchingComputerCategory.CompTitle} has been successfully removed.");

                    if (matchingComputerCategory.Count != 0)
                    {
                        continue;
                    }

                    computerCategoryList.Remove(matchingComputerCategory);
                    Console.WriteLine($"Category of computers {matchingComputerCategory.CompTitle} has been removed.");
                }
                else
                {
                    throw new Exception($"Computer with the title {computer.Title} was not found.");
                }
            }

            UpdateComputerList(computerCategoryList);
        }

        public void ParseCompTitlesAndAddCompCategories()
        {
            if (!File.Exists(_computerTitlesFilePath))
            {
                FileHelper.NoFileReport_KillProcess(_computerTitlesFilePath);
            }

            if (!File.Exists(_filePath))
            {
                FileHelper.NoFileReport_KillProcess(_filePath);
            }

            try
            {
                var compTitles = File.ReadAllLines(_computerTitlesFilePath);
                var random = new Random();

                var compCategoryList = compTitles.Select(t =>
                    new ComputerCategory
                    {
                        CompTitle = t.Trim(),
                        Price = random.Next(100, 1000),
                        Count = random.Next(1, 10)
                    }).ToList();

                var json = JsonSerializer.Serialize(compCategoryList);
                WriteFileContent(_filePath, json);
                Console.WriteLine("ComputerCategory objects have been successfully written to the file.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error writing to the file: " + ex.Message);
            }
        }
    }
}