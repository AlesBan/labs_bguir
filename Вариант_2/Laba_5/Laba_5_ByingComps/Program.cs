using System;
using System.IO;
using Laba_5_ByingComps.Helpers;
using Laba_5_ByingComps.Models;

namespace Laba_5_ByingComps
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var directory = Directory.GetCurrentDirectory();
            var compStorageFilePath = Path.Combine(directory, "CompStorage.txt");
            var customCompTitlesFilePath = Path.Combine(directory, "CompTitles.txt");

            var computerStorage = new ComputerStorage(compStorageFilePath, customCompTitlesFilePath);
            var computerShopManager = new CompKeeper(computerStorage);
            var purchaseLogger = new FileLogger();

            computerStorage.ParseCompTitlesAndAddCompCategories();

            while (true)
            {
                Console.WriteLine("What can I do for you?");
                Console.WriteLine("1. Show available computers");
                Console.WriteLine("2. Order computers");
                Console.WriteLine("3. Remove computers from the warehouse");
                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UserCommunicator.GetAllComputers(computerShopManager);
                        break;

                    case "2":
                        UserCommunicator.OrderComputers(computerShopManager, purchaseLogger);
                        break;

                    case "3":
                        UserCommunicator.DeleteComputers(computerShopManager, purchaseLogger);
                        break;

                    case "4":
                        Console.WriteLine("Thank you for visiting our computer shop. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}