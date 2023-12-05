using System;
using System.Collections.Generic;
using System.Linq;
using Laba_5_TvSales.Models.Store;

namespace Laba_5_TvSales.Models
{
    public static class Customer
    {
        public static void BuyTvs(TvStoreManager saleManager, TvStore tvShopRepository, TvStoreLogger salesLogger)
        {
            var selectedTvs = new List<Tv>();

            while (true)
            {
                var selectedTvCategory = GetSelectedTvCategory(tvShopRepository);

                Console.WriteLine(
                    $"Title: {selectedTvCategory?.Title}, Price: {selectedTvCategory?.Price}, Quantity in stock: {selectedTvCategory?.Count}");

                var quantity = GetValidQuantity(selectedTvCategory);

                var tvsToBuy = CreateTvsToBuy(selectedTvCategory, quantity);
                selectedTvs.AddRange(tvsToBuy);
                Console.WriteLine($"Selected: {quantity}, Total cost: {selectedTvCategory?.Price * quantity}");
                Console.WriteLine("TVs successfully added to the cart!");

                if (!ContinueSelectingTvs())
                {
                    break;
                }
            }

            if (selectedTvs.Count > 0)
            {
                Console.WriteLine("You have selected the following TVs:");
                DisplaySelectedTvs(selectedTvs);
                Console.WriteLine("Total cost: " + CalculateTotalCost(selectedTvs));

                if (ConfirmPurchase())
                {
                    CompletePurchase(saleManager, selectedTvs, salesLogger);
                }
                else
                {
                    Console.WriteLine("Purchase canceled.");
                }
            }
            else
            {
                Console.WriteLine("You haven't selected any TVs. Purchase canceled.");
            }
        }

        private static TvCategory GetSelectedTvCategory(TvStore tvStore)
        {
            while (true)
            {
                Console.Write("Enter the title of the TV you want to buy (or 'e' to finish the purchase): ");
                var tvTitle = Console.ReadLine()?.Trim();

                if (tvTitle == "e")
                {
                    break;
                }

                var selectedTvCategory = tvStore.GetTvCategory(tvTitle);

                if (selectedTvCategory == null)
                {
                    Console.WriteLine("Invalid TV title!");
                }
                else
                {
                    return selectedTvCategory;
                }
            }

            return null;
        }

        private static int GetValidQuantity(TvCategory selectedTvCategory)
        {
            int quantity;

            while (true)
            {
                Console.Write("Enter the quantity you want to buy: ");
                if (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity!");
                }
                else if (selectedTvCategory != null && quantity > selectedTvCategory.Count)
                {
                    Console.WriteLine(
                        "The specified quantity exceeds the available quantity in stock. Please choose a smaller quantity.");
                }
                else
                {
                    break;
                }
            }

            return quantity;
        }

        private static List<Tv> CreateTvsToBuy(TvCategory selectedTvCategory, int quantity)
        {
            var tvsToBuy = new List<Tv>();

            if (selectedTvCategory == null)
            {
                return tvsToBuy;
            }

            for (var i = 0; i < quantity; i++)
            {
                var tv = new Tv { Title = selectedTvCategory.Title, Price = selectedTvCategory.Price };
                tvsToBuy.Add(tv);
            }

            return tvsToBuy;
        }

        private static bool ContinueSelectingTvs()
        {
            Console.Write("Do you want to continue selecting TVs? (Y/N): ");
            var input = Console.ReadLine()?.Trim();
            return input?.ToLower() == "y";
        }

        private static void DisplaySelectedTvs(List<Tv> selectedTvs)
        {
            foreach (var tv in selectedTvs)
            {
                Console.WriteLine($"Title: {tv.Title}, Price: {tv.Price}");
            }
        }

        private static double CalculateTotalCost(List<Tv> selectedTvs)
        {
            var totalPrice = 0.0;

            foreach (var tv in selectedTvs)
            {
                totalPrice += tv.Price;
            }

            return totalPrice;
        }

        private static bool ConfirmPurchase()
        {
            Console.Write("Confirm the purchase (Y/N): ");
            var input = Console.ReadLine()?.Trim();
            return input?.ToLower() == "y";
        }

        private static void CompletePurchase(TvStoreManager saleManager, List<Tv> selectedTvs,
            TvStoreLogger salesLogger)
        {
            saleManager.SellTvs(selectedTvs);
            Console.WriteLine("Purchase completed successfully!");

            var tvTitleList = selectedTvs.Select(tv => tv.Title).ToList();

            var totalPrice = CalculateTotalCost(selectedTvs);
            salesLogger.LogSale(tvTitleList, totalPrice);
        }
    }
}