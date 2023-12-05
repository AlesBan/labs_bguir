using System;
using System.Collections.Generic;
using System.Linq;
using Laba_5_ByingComps.Interfaces;
using Laba_5_ByingComps.Models;

namespace Laba_5_ByingComps.Helpers;

public static class UserCommunicator
{
    public static void GetAllComputers(ICompKeeper shopManager)
    {
        var availableComputers = shopManager.GetAllComputers();
        Console.WriteLine("Available computers:");
        foreach (var computer in availableComputers)
        {
            Console.WriteLine(computer.FullInfo());
        }
    }

    public static void OrderComputers(ICompKeeper shopManager, IFileLogger purchaseLogger)
    {
        var selectedComputers = new List<Computer>();

        while (true)
        {
            ComputerCategory selectedComputerCategory = null;

            while (selectedComputerCategory == null)
            {
                Console.Write("Enter the model of the computer you want to order (or 'q' to finish the order): ");
                var computerTitle = Console.ReadLine()?.Trim() ?? "";

                if (computerTitle == "q")
                {
                    break;
                }

                selectedComputerCategory = shopManager.GetComputerCategoryByTitle(computerTitle);

                if (selectedComputerCategory == null)
                {
                    Console.WriteLine("Invalid computer model!");
                }
            }

            Console.WriteLine(
                $"Model: {selectedComputerCategory!.CompTitle}, Price: {selectedComputerCategory.Price}, Quantity in stock: {selectedComputerCategory.Count}");

            int quantity;
            while (true)
            {
                Console.Write("Enter the quantity you want to order: ");
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity!");
                }
                else
                {
                    break;
                }
            }

            var computersToOrder = Enumerable.Range(0, quantity)
                .Select(_ => new Computer
                    { Title = selectedComputerCategory.CompTitle, Price = selectedComputerCategory.Price })
                .ToList();

            selectedComputers.AddRange(computersToOrder);
            Console.WriteLine($"Selected: {quantity}, Total cost: {selectedComputerCategory.Price * quantity}");
            Console.WriteLine("Computers successfully added to the order!");

            Console.Write("Do you want to continue selecting computers? (Y/N): ");
            if (!IsPositiveResponse(Console.ReadLine()?.Trim()))
            {
                break;
            }
        }

        if (!selectedComputers.Any())
        {
            return;
        }

        Console.WriteLine("You have selected the following computers:");
        foreach (var computer in selectedComputers)
        {
            Console.WriteLine($"Model: {computer.Title}, Price: {computer.Price}");
        }

        Console.WriteLine("Total cost: " + selectedComputers.Sum(computer => computer.Price));

        Console.Write("Confirm the order? (Y/N): ");
        if (!IsPositiveResponse(Console.ReadLine()?.Trim()))
        {
            return;
        }

        shopManager.AddComputers(selectedComputers);
        Console.WriteLine("Order successfully placed!");

        var purchaseTotal = selectedComputers.Sum(computer => computer.Price);
        purchaseLogger.Log(selectedComputers.Select(c => c.Title), purchaseTotal);
    }

    public static void DeleteComputers(ICompKeeper purchaseManager, FileLogger purchaseLogger)
    {
        var selectedComputers = new List<Computer>();
        while (true)
        {
            ComputerCategory selectedComputerCategory = null;

            while (selectedComputerCategory == null)
            {
                Console.Write(
                    "Enter the model of the computer you want to write off (or 'q' to finish writing off): ");
                var computerTitle = Console.ReadLine() ?? "".Trim();

                if (computerTitle == "q")
                {
                    break;
                }

                selectedComputerCategory = purchaseManager.GetComputerCategoryByTitle(computerTitle);

                if (selectedComputerCategory == null)
                {
                    Console.WriteLine("Invalid computer model!");
                }
            }

            if (selectedComputerCategory == null)
            {
                break;
            }

            Console.WriteLine(
                $"Model: {selectedComputerCategory.CompTitle}, Price: {selectedComputerCategory.Price}, Quantity in stock: {selectedComputerCategory.Count}");

            int quantity;
            while (true)
            {
                Console.Write("Enter the quantity you want to write off: ");
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity!");
                }
                else if (quantity > selectedComputerCategory.Count)
                {
                    Console.WriteLine("Insufficient quantity in stock!");
                }
                else
                {
                    break;
                }
            }

            var compsToWriteOff = Enumerable.Range(0, quantity)
                .Select(_ => new Computer()
                    { Title = selectedComputerCategory.CompTitle, Price = selectedComputerCategory.Price })
                .ToList();

            purchaseManager.RemoveComputers(compsToWriteOff);
            Console.WriteLine("Computers successfully written off from the warehouse!");

            Console.Write("Do you want to continue writing off computers? (Y/N): ");
            if (!IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
            {
                break;
            }
        }

        if (selectedComputers.Any())
        {
            Console.WriteLine("You have written off the following computers:");
            foreach (var computer in selectedComputers)
            {
                Console.WriteLine($"Model: {computer.Title}, Price: {computer.Price}");
            }

            Console.Write("Confirm the removing? (Y/N): ");
            if (IsPositiveResponse(Console.ReadLine() ?? "".Trim()))
            {
                purchaseManager.RemoveComputers(selectedComputers);
                Console.WriteLine("Removing successfully executed!");

                purchaseLogger.Log(selectedComputers.Select(c => c.Title),
                    selectedComputers.Sum(computer => computer.Price));
            }
            else
            {
                Console.WriteLine("Removing canceled.");
            }
        }
        else
        {
            Console.WriteLine("You haven't selected any computers. Removing is canceled.");
        }
    }

    private static bool IsPositiveResponse(string input)
    {
        return input.ToLowerInvariant() is "y" or "yes";
    }
}