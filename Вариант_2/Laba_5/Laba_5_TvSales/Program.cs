using System;
using System.IO;
using System.Linq;
using Laba_5_TvSales.Models;
using Laba_5_TvSales.Models.Store;
using static System.Int32;

namespace Laba_5_TvSales
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            var tvStorageFilePath = Path.Combine(directory, "tvStorage.txt");
            var customTvTitlesFilePath = Path.Combine(directory, "tvTitles.txt");

            var tvStore = new TvStore(tvStorageFilePath, customTvTitlesFilePath);
            var tvStoreManager = new TvStoreManager(tvStore);
            var salesLogger = new TvStoreLogger();

            tvStore.InitializeTvCategories();
            
            while (true)
            {
                Console.WriteLine("Welcome to our amazing TV store!");
                Console.WriteLine("How can I assist you today?");
                Console.WriteLine("1. View the list of available TVs");
                Console.WriteLine("2. Purchase a TV");
                Console.WriteLine("3. Add new TVs to the inventory");
                Console.WriteLine("4. Exit the store");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("Fetching the list of available TVs...\n");
                    var availableTvCategories = tvStoreManager.GetAllTvCategories();
                    var tvCategories = availableTvCategories.ToList();
                    if (!tvCategories.Any())
                    {
                        Console.WriteLine("Oops! There are no TVs available at the moment.");
                    }
                    else
                    {
                        Console.WriteLine("Available TVs:\n");
                        foreach (var tvCategory in tvCategories)
                        {
                            Console.WriteLine(tvCategory.GetFullInfo());
                        }
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Great! Let's proceed with your TV purchase.\n");
                    Customer.BuyTvs(tvStoreManager, tvStore, salesLogger);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Adding new TVs to the inventory...\n");
                    Console.Write("Enter the title of the new TV: ");
                    var newTvTitle = Console.ReadLine()?.Trim();

                    Console.Write("Enter the price of the new TV: ");
                    var priceStr = Console.ReadLine() ?? string.Empty;
                    TryParse(priceStr, out var newTvPrice);

                    Console.Write("Enter the quantity of the new TVs: ");
                    var countStr = Console.ReadLine() ?? string.Empty;
                    TryParse(countStr, out var newTvCount);

                    var newTvCategory = new TvCategory()
                    {
                        Title = newTvTitle,
                        Price = newTvPrice,
                        Count = newTvCount
                    };

                    tvStoreManager.AddTvCategory(newTvCategory);
                    Console.WriteLine("\nNew TV added to the inventory successfully!");
                }
                else if (choice == "4")
                {
                    Console.WriteLine("Thank you for visiting our TV store. Have a great day!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice! Please try again.\n");
                }

                Console.WriteLine();
            }
        }
    }
}